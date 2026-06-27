using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

public partial class IncidentesAdminPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public IncidentesAdminPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int usuarioId = Preferences.Get("UsuarioId", 0);
        var incidentes = await _apiService.ObtenerIncidentesAdminAsync(usuarioId);

        IncidentesCollection.ItemsSource = incidentes;
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}