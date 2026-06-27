using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

public partial class DashboardPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public DashboardPage()
    {
        InitializeComponent();

        string nombre = Preferences.Get("UsuarioNombre", "Usuario");
        BienvenidaLabel.Text = $"Bienvenido, {nombre}";
        bool esAdmin = Preferences.Get("EsAdmin", false);
        AdminTabsLayout.IsVisible = esAdmin;

        CargarMapa();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarAlertasRecientes();
    }

    private async Task CargarAlertasRecientes()
    {
        var alertas = await _apiService.ObtenerAlertasRecientesAsync();
        AlertasCollection.ItemsSource = alertas;
    }

    private async void OnAdminClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AdminDashboardPage));
    }

    private async void OnUsuariosAdminClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UsuariosAdminPage));
    }

    private async void OnIncidentesAdminClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(IncidentesAdminPage));
    }

    private async void OnAlertasSOSAdminClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AlertasSOSAdminPage));
    }
    private void CargarMapa()
    {
        string html = """
        <!DOCTYPE html>
        <html>
        <head>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css'/>
            <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
            <style>
                html, body, #map { height:100%; margin:0; padding:0; }
            </style>
        </head>
        <body>
            <div id='map'></div>

            <script>
                var map = L.map('map').setView([-0.2299, -78.5249], 12);

                L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                    maxZoom: 19
                }).addTo(map);

                var zonas = [
                    ['Quitumbe - Riesgo Alto', -0.2917, -78.5461, 'red'],
                    ['Solanda - Riesgo Alto', -0.2464, -78.5296, 'red'],
                    ['La Mariscal - Riesgo Alto', -0.2021, -78.4900, 'red'],
                    ['Villaflora - Riesgo Medio', -0.2495, -78.5228, 'orange'],
                    ['San Roque - Riesgo Medio', -0.2219, -78.5204, 'orange'],
                    ['Parque La Carolina - Punto Seguro', -0.1807, -78.4678, 'green']
                ];

                zonas.forEach(function(z) {
                    L.circle([z[1], z[2]], {
                        color: z[3],
                        fillColor: z[3],
                        fillOpacity: 0.35,
                        radius: 500
                    }).addTo(map).bindPopup(z[0]);
                });
            </script>
        </body>
        </html>
        """;

        MapaWebView.Source = new HtmlWebViewSource
        {
            Html = html
        };
    }

    private async void OnSOSClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SOSPage));
    }

    private async void OnMisReportesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MisReportesPage));
    }
    private async void OnReportarIncidenteClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReportarIncidentePage));
    }
    private async void OnAsistenciaClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AsistenciaPage));
    }
    private async void OnNotificacionesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NotificacionesPage));
    }

    private async void OnPerfilClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PerfilPage));
    }
    private async void OnSalirClicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert(
            "Cerrar sesión",
            "¿Seguro que deseas salir?",
            "Sí",
            "Cancelar");

        if (!confirmar)
            return;

        Preferences.Remove("UsuarioId");
        Preferences.Remove("UsuarioNombre");
        Preferences.Remove("UsuarioCorreo");
        Preferences.Remove("EsAdmin");

        await Shell.Current.GoToAsync("//LoginPage");
    }

}