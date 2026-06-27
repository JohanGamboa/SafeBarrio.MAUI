using Plugin.Maui.Audio;
using SafeBarrio.MAUI.Models;
using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

public partial class SOSPage : ContentPage
{
    private double _latitud;
    private double _longitud;
    private int _sosActivoId;

    private readonly ApiService _apiService = new ApiService();
    private readonly IAudioManager _audioManager;

    public SOSPage()
    {
        InitializeComponent();
        _audioManager = AudioManager.Current;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await ObtenerUbicacion();
        await CargarMiSOSActivo();
    }

    private async Task ObtenerUbicacion()
    {
        try
        {
            var permiso = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (permiso != PermissionStatus.Granted)
            {
                permiso = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (permiso != PermissionStatus.Granted)
            {
                UbicacionLabel.Text = "Permiso de ubicación denegado.";
                return;
            }

            var request = new GeolocationRequest(
                GeolocationAccuracy.Best,
                TimeSpan.FromSeconds(20));

            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                _latitud = location.Latitude;
                _longitud = location.Longitude;
                UbicacionLabel.Text = $"Ubicación real: {_latitud}, {_longitud}";
            }
            else
            {
                UbicacionLabel.Text = "No se pudo obtener la ubicación real.";
            }
        }
        catch (Exception ex)
        {
            UbicacionLabel.Text = "Error al obtener ubicación: " + ex.Message;
        }
    }

    private async Task CargarMiSOSActivo()
    {
        int usuarioId = Preferences.Get("UsuarioId", 0);

        if (usuarioId == 0)
            return;

        AlertaSOSModel? sos = await _apiService.ObtenerMiSOSActivoAsync(usuarioId);

        if (sos != null && sos.id > 0)
        {
            _sosActivoId = sos.id;
            SOSActivoBox.IsVisible = true;
            SOSActivoLabel.Text = $"Estado: {sos.estado}\nFecha: {sos.fechaAlerta}\n{sos.mensaje}";
        }
        else
        {
            _sosActivoId = 0;
            SOSActivoBox.IsVisible = false;
        }
    }

    private async void OnEnviarSOSClicked(object sender, EventArgs e)
    {
        int usuarioId = Preferences.Get("UsuarioId", 0);

        if (usuarioId == 0)
        {
            await DisplayAlert("Error", "No se encontró el usuario logueado.", "OK");
            return;
        }

        if (_latitud == 0 || _longitud == 0)
        {
            await ObtenerUbicacion();
        }

        if (_latitud == 0 || _longitud == 0)
        {
            await DisplayAlert(
                "Ubicación",
                "No se pudo obtener tu ubicación real. Activa el GPS y concede permisos de ubicación.",
                "OK");

            return;
        }

        var resultado = await _apiService.GuardarSOSAsync(usuarioId, _latitud, _longitud);

        if (resultado != null && resultado.ok)
        {
            try
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(700));
            }
            catch { }

            await ReproducirAlerta();

            await DisplayAlert(
                "SOS",
                resultado.mensaje ?? "Alerta SOS enviada correctamente.",
                "OK");

            await CargarMiSOSActivo();
        }
        else
        {
            await DisplayAlert(
                "Error",
                resultado?.mensaje ?? "No se pudo enviar la alerta SOS.",
                "OK");
        }
    }

    private async void OnFinalizarSOSClicked(object sender, EventArgs e)
    {
        int usuarioId = Preferences.Get("UsuarioId", 0);

        if (usuarioId == 0 || _sosActivoId == 0)
        {
            await DisplayAlert("Error", "No se encontró una alerta activa.", "OK");
            return;
        }

        bool confirmar = await DisplayAlert(
            "Finalizar SOS",
            "¿Seguro que deseas finalizar tu alerta SOS?",
            "Sí",
            "Cancelar");

        if (!confirmar)
            return;

        var resultado = await _apiService.FinalizarSOSAsync(usuarioId, _sosActivoId);

        if (resultado != null && resultado.ok)
        {
            await DisplayAlert("SOS", resultado.mensaje ?? "Alerta SOS finalizada.", "OK");
            await CargarMiSOSActivo();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo finalizar la alerta SOS.", "OK");
        }
    }

    private async Task ReproducirAlerta()
    {
        try
        {
            using var file = await FileSystem.OpenAppPackageFileAsync("alerta.mp3");
            var player = _audioManager.CreatePlayer(file);
            player.Play();
        }
        catch
        {
        }
    }

    private async void OnLlamarClicked(object sender, EventArgs e)
    {
        try
        {
            PhoneDialer.Open("911");
        }
        catch
        {
            await DisplayAlert("Error", "No se pudo abrir el marcador telefónico.", "OK");
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}