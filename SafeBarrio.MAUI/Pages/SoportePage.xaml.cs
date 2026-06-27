namespace SafeBarrio.MAUI.Pages;

public partial class SoportePage : ContentPage
{
    public SoportePage()
    {
        InitializeComponent();
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}