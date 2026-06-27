using SafeBarrio.MAUI.Services;

namespace SafeBarrio.MAUI.Pages;

public partial class ReportarIncidentePage : ContentPage
{
    private double _latitud = -0.2299;
    private double _longitud = -78.5249;
    private FileResult? _imagenSeleccionada;
    private readonly ApiService _apiService = new ApiService();

    public ReportarIncidentePage()
    {
        InitializeComponent();
        CargarMapa(_latitud, _longitud, "Quito");
        ActualizarTextoUbicacion();
    }

    private void CargarMapa(double lat, double lng, string titulo)
    {
        string latText = lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
        string lngText = lng.ToString(System.Globalization.CultureInfo.InvariantCulture);

        string html = $$"""
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
                var map = L.map('map').setView([{{latText}}, {{lngText}}], 15);

                L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                    maxZoom: 19
                }).addTo(map);

                var marker = L.marker([{{latText}}, {{lngText}}])
                    .addTo(map)
                    .bindPopup('{{titulo}}')
                    .openPopup();

                map.on('click', function(e) {
                    var lat = e.latlng.lat;
                    var lng = e.latlng.lng;

                    marker.setLatLng([lat, lng]);
                    window.location.href = 'safebarrio://seleccionar?lat=' + lat + '&lng=' + lng;
                });
            </script>
        </body>
        </html>
        """;

        MapaWebView.Source = new HtmlWebViewSource { Html = html };
    }

    private void MapaWebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        if (e.Url.StartsWith("safebarrio://seleccionar"))
        {
            e.Cancel = true;

            var uri = new Uri(e.Url);
            string query = uri.Query.Replace("?", "");
            var partes = query.Split('&');

            foreach (var parte in partes)
            {
                var valores = parte.Split('=');

                if (valores.Length == 2)
                {
                    if (valores[0] == "lat")
                        double.TryParse(valores[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out _latitud);

                    if (valores[0] == "lng")
                        double.TryParse(valores[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out _longitud);
                }
            }

            ActualizarTextoUbicacion();
        }
    }

    private void ActualizarTextoUbicacion()
    {
        UbicacionLabel.Text = $"Ubicación seleccionada: {_latitud}, {_longitud}";
    }

    private async void OnBuscarDireccionClicked(object sender, EventArgs e)
    {
        string direccion = (DireccionEntry.Text ?? "").ToLower();

        var zonasQuito = new Dictionary<string, (double lat, double lng)>
        {
            { "san roque", (-0.2219, -78.5204) },
            { "villaflora", (-0.2495, -78.5228) },
            { "solanda", (-0.2464, -78.5296) },
            { "quitumbe", (-0.2917, -78.5461) },
            { "la mariscal", (-0.2021, -78.4900) },
            { "recoleta", (-0.2292, -78.5138) }
        };

        foreach (var zona in zonasQuito)
        {
            if (direccion.Contains(zona.Key))
            {
                _latitud = zona.Value.lat;
                _longitud = zona.Value.lng;
                CargarMapa(_latitud, _longitud, zona.Key);
                ActualizarTextoUbicacion();
                return;
            }
        }

        await DisplayAlert("SafeBarrio", "No se encontró esa zona. Puedes tocar el mapa manualmente.", "OK");
    }

    private async void OnUbicacionClicked(object sender, EventArgs e)
    {
        try
        {
            var location = await Geolocation.GetLocationAsync(
                new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));

            if (location != null)
            {
                _latitud = location.Latitude;
                _longitud = location.Longitude;
                CargarMapa(_latitud, _longitud, "Mi ubicación");
                ActualizarTextoUbicacion();
            }
        }
        catch
        {
            await DisplayAlert("Ubicación", "No se pudo obtener la ubicación. Puedes seleccionar en el mapa.", "OK");
        }
    }

    private async void OnSeleccionarFotoClicked(object sender, EventArgs e)
    {
        try
        {
            _imagenSeleccionada = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una foto de evidencia",
                FileTypes = FilePickerFileType.Images
            });

            if (_imagenSeleccionada != null)
            {
                FotoButton.Text = "Foto seleccionada: " + _imagenSeleccionada.FileName;

                var stream = await _imagenSeleccionada.OpenReadAsync();
                VistaFoto.Source = ImageSource.FromStream(() => stream);
                VistaFoto.IsVisible = true;
            }
        }
        catch
        {
            await DisplayAlert("Foto", "No se pudo seleccionar la imagen.", "OK");
        }
    }

    private async void OnPublicarClicked(object sender, EventArgs e)
    {
        int usuarioId = Preferences.Get("UsuarioId", 0);

        if (TipoPicker.SelectedItem == null ||
            string.IsNullOrWhiteSpace(DescripcionEditor.Text) ||
            string.IsNullOrWhiteSpace(DireccionEntry.Text))
        {
            await DisplayAlert("Validación", "Completa todos los campos.", "OK");
            return;
        }

        var resultado = await _apiService.ReportarIncidenteAsync(
            usuarioId,
            TipoPicker.SelectedItem.ToString() ?? "",
            DescripcionEditor.Text,
            DireccionEntry.Text,
            _latitud,
            _longitud,
            _imagenSeleccionada);

        if (resultado != null && resultado.ok)
        {
            await DisplayAlert("SafeBarrio", "Incidente reportado correctamente.", "OK");
            await Shell.Current.GoToAsync(nameof(MisReportesPage));
        }
        else
        {
            await DisplayAlert("Error", "No se pudo publicar el reporte.", "OK");
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}