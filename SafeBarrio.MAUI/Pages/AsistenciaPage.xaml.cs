using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

public partial class AsistenciaPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public AsistenciaPage()
    {
        InitializeComponent();
    }

    private async void OnEnviarClicked(object sender, EventArgs e)
    {
        string mensaje = MensajeEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(mensaje))
        {
            await DisplayAlert("SafeBarrio", "Escribe un mensaje.", "OK");
            return;
        }

        ChatLayout.Children.Add(new Label
        {
            Text = "Tú: " + mensaje,
            TextColor = Colors.DarkGreen,
            FontAttributes = FontAttributes.Bold
        });

        MensajeEntry.Text = "";

        var respuesta = await _apiService.EnviarMensajeAsistenciaAsync(mensaje);

        ChatLayout.Children.Add(new Label
        {
            Text = "SafeBot: " + (respuesta?.respuesta ?? "No pude responder en este momento."),
            TextColor = Colors.Black
        });
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}