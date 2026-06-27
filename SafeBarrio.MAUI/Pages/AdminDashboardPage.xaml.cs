using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

public partial class AdminDashboardPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public AdminDashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int usuarioId = Preferences.Get("UsuarioId", 0);
        var data = await _apiService.ObtenerAdminDashboardAsync(usuarioId);

        if (data == null || !data.ok)
        {
            await DisplayAlert("Error", "No se pudo cargar el panel administrador.", "OK");
            return;
        }

        UsuariosLabel.Text = data.totalUsuarios.ToString();
        IncidentesLabel.Text = data.totalIncidentes.ToString();
        SOSActivosLabel.Text = data.sosActivos.ToString();
        SOSResueltosLabel.Text = data.sosResueltos.ToString();
    }

    private async void OnUsuariosClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UsuariosAdminPage));
    }

    private async void OnIncidentesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(IncidentesAdminPage));
    }

    private async void OnAlertasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AlertasSOSAdminPage));
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}