using Microsoft.Maui.Storage;
using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string correo = CorreoEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("SafeBarrio", "Ingresa correo y contraseña.", "OK");
            return;
        }

        var resultado = await _apiService.LoginAsync(correo, password);

        if (resultado != null && resultado.ok)
        {
            Preferences.Set("UsuarioId", resultado.id);
            Preferences.Set("UsuarioNombre", resultado.nombre ?? "");
            Preferences.Set("UsuarioCorreo", resultado.correo ?? "");
            Preferences.Set("EsAdmin", resultado.esAdmin);

            await Shell.Current.GoToAsync(nameof(DashboardPage));
        }
        else
        {
            await DisplayAlert(
                "Error",
                resultado?.mensaje ?? "No se pudo conectar con la API.",
                "OK");
        }
    }

    private async void OnRegistroClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegistroPage));
    }

    private async void OnSoporteClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SoportePage));
    }
}