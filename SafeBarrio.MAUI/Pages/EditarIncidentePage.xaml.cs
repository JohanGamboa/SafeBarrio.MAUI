using SafeBarrio.MAUI.Models;
using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

[QueryProperty(nameof(IncidenteId), "id")]
public partial class EditarIncidentePage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    private int _id;
    private double _latitud;
    private double _longitud;
    private FileResult? _imagenSeleccionada;

    public string IncidenteId
    {
        set
        {
            if (int.TryParse(value, out int id))
            {
                _id = id;
            }
        }
    }

    public EditarIncidentePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarReporte();
    }

    private async Task CargarReporte()
    {
        int usuarioId = Preferences.Get("UsuarioId", 0);

        IncidenteModel? incidente = await _apiService.ObtenerIncidenteAsync(_id, usuarioId);

        if (incidente == null)
        {
            await DisplayAlert("Error", "No se pudo cargar el reporte.", "OK");
            await Shell.Current.GoToAsync("..");
            return;
        }

        TipoPicker.SelectedItem = incidente.tipoIncidente;
        DescripcionEditor.Text = incidente.descripcion;
        DireccionEntry.Text = incidente.direccionReferencia;

        _latitud = incidente.latitud;
        _longitud = incidente.longitud;

        UbicacionLabel.Text = $"Ubicación: {_latitud}, {_longitud}";

        if (!string.IsNullOrWhiteSpace(incidente.imagenRuta))
        {
            VistaFoto.Source = incidente.imagenRuta;
            VistaFoto.IsVisible = true;
        }
    }

    private async void OnSeleccionarFotoClicked(object sender, EventArgs e)
    {
        try
        {
            _imagenSeleccionada = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una nueva evidencia",
                FileTypes = FilePickerFileType.Images
            });

            if (_imagenSeleccionada != null)
            {
                var stream = await _imagenSeleccionada.OpenReadAsync();
                VistaFoto.Source = ImageSource.FromStream(() => stream);
                VistaFoto.IsVisible = true;
            }
        }
        catch
        {
            await DisplayAlert("Foto", "No se pudo seleccionar la imagen.", "OK");
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        int usuarioId = Preferences.Get("UsuarioId", 0);

        if (TipoPicker.SelectedItem == null ||
            string.IsNullOrWhiteSpace(DescripcionEditor.Text) ||
            string.IsNullOrWhiteSpace(DireccionEntry.Text))
        {
            await DisplayAlert("Validación", "Completa todos los campos.", "OK");
            return;
        }

        var resultado = await _apiService.EditarIncidenteAsync(
            _id,
            usuarioId,
            TipoPicker.SelectedItem.ToString() ?? "",
            DescripcionEditor.Text,
            DireccionEntry.Text,
            _latitud,
            _longitud,
            _imagenSeleccionada);

        if (resultado != null && resultado.ok)
        {
            await DisplayAlert("SafeBarrio", resultado.mensaje ?? "Reporte actualizado.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Error", "No se pudo actualizar el reporte.", "OK");
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}