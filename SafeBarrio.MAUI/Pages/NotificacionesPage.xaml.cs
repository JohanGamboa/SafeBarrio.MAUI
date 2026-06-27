using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

public partial class NotificacionesPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public NotificacionesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int usuarioId = Preferences.Get("UsuarioId", 0);
        var notificaciones = await _apiService.ObtenerNotificacionesAsync(usuarioId);

        NotificacionesCollection.ItemsSource = notificaciones;
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}