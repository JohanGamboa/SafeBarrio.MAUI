using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

public partial class MisReportesPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public MisReportesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarReportes();
    }

    private async Task CargarReportes()
    {
        int usuarioId = Preferences.Get("UsuarioId", 0);
        var reportes = await _apiService.ObtenerMisReportesAsync(usuarioId);
        ReportesCollection.ItemsSource = reportes;
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter != null)
        {
            int id = Convert.ToInt32(btn.CommandParameter);
            await Shell.Current.GoToAsync($"{nameof(EditarIncidentePage)}?id={id}");
        }
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter != null)
        {
            int id = Convert.ToInt32(btn.CommandParameter);
            int usuarioId = Preferences.Get("UsuarioId", 0);

            bool confirmar = await DisplayAlert(
                "Eliminar reporte",
                "¿Seguro que deseas eliminar este reporte?",
                "Sí",
                "Cancelar");

            if (!confirmar)
                return;

            var resultado = await _apiService.EliminarIncidenteAsync(id, usuarioId);

            if (resultado != null && resultado.ok)
            {
                await DisplayAlert("SafeBarrio", resultado.mensaje ?? "Reporte eliminado.", "OK");
                await CargarReportes();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo eliminar el reporte.", "OK");
            }
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}