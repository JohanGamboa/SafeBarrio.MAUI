using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

public partial class AlertasSOSAdminPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public AlertasSOSAdminPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int usuarioId = Preferences.Get("UsuarioId", 0);
        var alertas = await _apiService.ObtenerAlertasSOSAdminAsync(usuarioId);

        AlertasCollection.ItemsSource = alertas;
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}