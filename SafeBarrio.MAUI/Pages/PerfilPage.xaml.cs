using SafeBarrio.MAUI.Models;
using SafeBarrio.MAUI.Services;


namespace SafeBarrio.MAUI.Pages;

public partial class PerfilPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();
    private UsuarioModel? _usuarioActual;

    public PerfilPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarPerfil();
    }

    private async Task CargarPerfil()
    {
        int usuarioId = Preferences.Get("UsuarioId", 0);

        _usuarioActual = await _apiService.ObtenerPerfilAsync(usuarioId);

        if (_usuarioActual == null)
        {
            await DisplayAlert("Error", "No se pudo cargar el perfil.", "OK");
            return;
        }

        NombreLabel.Text = $"{_usuarioActual.nombre} {_usuarioActual.apellido}";
        CorreoLabel.Text = $"Correo: {_usuarioActual.correo}";
        TelefonoLabel.Text = $"Teléfono: {_usuarioActual.telefono}";
        UbicacionLabel.Text = $"Ubicación: {_usuarioActual.ubicacion}";
        RolLabel.Text = _usuarioActual.esAdmin ? "Rol: Administrador" : "Rol: Usuario";

        NombreEntry.Text = _usuarioActual.nombre;
        ApellidoEntry.Text = _usuarioActual.apellido;
        TelefonoEntry.Text = _usuarioActual.telefono;
        UbicacionEntry.Text = _usuarioActual.ubicacion;

        EditarLayout.IsVisible = false;
        EditarButton.IsVisible = true;
    }

    private void OnEditarClicked(object sender, EventArgs e)
    {
        EditarLayout.IsVisible = true;
        EditarButton.IsVisible = false;
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        int usuarioId = Preferences.Get("UsuarioId", 0);

        var resultado = await _apiService.EditarPerfilAsync(
            usuarioId,
            NombreEntry.Text ?? "",
            ApellidoEntry.Text ?? "",
            TelefonoEntry.Text ?? "",
            UbicacionEntry.Text ?? "");

        if (resultado != null && resultado.ok)
        {
            Preferences.Set("UsuarioNombre", NombreEntry.Text ?? "");

            await DisplayAlert("SafeBarrio", resultado.mensaje ?? "Perfil actualizado.", "OK");

            await CargarPerfil();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo actualizar el perfil.", "OK");
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}