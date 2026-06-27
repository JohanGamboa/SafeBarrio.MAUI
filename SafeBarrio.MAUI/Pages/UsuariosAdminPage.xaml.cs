using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

public partial class UsuariosAdminPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public UsuariosAdminPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int usuarioId = Preferences.Get("UsuarioId", 0);
        var usuarios = await _apiService.ObtenerUsuariosAdminAsync(usuarioId);

        UsuariosCollection.ItemsSource = usuarios;
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}