using System.Net.Http.Json;
using SafeBarrio.MAUI.Models;



namespace SafeBarrio.MAUI.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

#if ANDROID
        private const string BaseUrl = "https://dispatch-educator-unveiling.ngrok-free.dev";
#else
    private const string BaseUrl = "https://localhost:44346";
    #endif


    public ApiService()
    {
        var handler = new HttpClientHandler();

        handler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        _httpClient = new HttpClient(handler);
        _httpClient.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");
    }

    public async Task<LoginResponse?> LoginAsync(string correo, string password)
    {
        try
        {
            var datos = new Dictionary<string, string>
            {
                { "correo", correo },
                { "password", password }
            };

            var response = await _httpClient.PostAsync($"{BaseUrl}/Api/Login", new FormUrlEncodedContent(datos));

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }
        catch
        {
            return null;
        }
    }

    public async Task<UsuarioModel?> ObtenerPerfilAsync(int usuarioId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<UsuarioModel>(
                $"{BaseUrl}/Api/Perfil?usuarioId={usuarioId}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<ApiResponse?> EditarPerfilAsync(
        int usuarioId,
        string nombre,
        string apellido,
        string telefono,
        string ubicacion)
    {
        try
        {
            var datos = new Dictionary<string, string>
        {
            { "usuarioId", usuarioId.ToString() },
            { "nombre", nombre },
            { "apellido", apellido },
            { "telefono", telefono },
            { "ubicacion", ubicacion }
        };

            var response = await _httpClient.PostAsync(
                $"{BaseUrl}/Api/EditarPerfil",
                new FormUrlEncodedContent(datos));

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ApiResponse>();
        }
        catch
        {
            return null;
        }
    }

    public async Task<AsistenciaResponse?> EnviarMensajeAsistenciaAsync(string mensaje)
    {
        try
        {
            var datos = new Dictionary<string, string>
        {
            { "mensaje", mensaje }
        };

            var response = await _httpClient.PostAsync(
                $"{BaseUrl}/Api/Asistencia",
                new FormUrlEncodedContent(datos));

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AsistenciaResponse>();
        }
        catch
        {
            return null;
        }
    }
    public async Task<ApiResponse?> ReportarIncidenteAsync(
    int usuarioId,
    string tipoIncidente,
    string descripcion,
    string direccionReferencia,
    double latitud,
    double longitud,
    FileResult? imagen)
    {
        try
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(usuarioId.ToString()), "usuarioId");
            content.Add(new StringContent(tipoIncidente), "tipoIncidente");
            content.Add(new StringContent(descripcion), "descripcion");
            content.Add(new StringContent(direccionReferencia), "direccionReferencia");
            content.Add(new StringContent(latitud.ToString(System.Globalization.CultureInfo.InvariantCulture)), "latitud");
            content.Add(new StringContent(longitud.ToString(System.Globalization.CultureInfo.InvariantCulture)), "longitud");

            if (imagen != null)
            {
                var stream = await imagen.OpenReadAsync();
                var fileContent = new StreamContent(stream);
                content.Add(fileContent, "imagenEvidencia", imagen.FileName);
            }

            var response = await _httpClient.PostAsync($"{BaseUrl}/Api/ReportarIncidente", content);
            var texto = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("API Error", texto, "OK");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ApiResponse>();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error reporte", ex.Message, "OK");
            return null;
        }
    }


    public async Task<ApiResponse?> GuardarSOSAsync(int usuarioId, double latitud, double longitud)
    {
        var datos = new Dictionary<string, string>
    {
        { "usuarioId", usuarioId.ToString() },
        { "latitud", latitud.ToString(System.Globalization.CultureInfo.InvariantCulture) },
        { "longitud", longitud.ToString(System.Globalization.CultureInfo.InvariantCulture) }
    };

        var response = await _httpClient.PostAsync(
            $"{BaseUrl}/Api/GuardarSOS",
            new FormUrlEncodedContent(datos));

        var texto = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            await Shell.Current.DisplayAlert("API Error", texto, "OK");
            return null;
        }

        return System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(
            texto,
            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<IncidenteModel?> ObtenerIncidenteAsync(int id, int usuarioId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IncidenteModel>(
                $"{BaseUrl}/Api/ObtenerIncidente?id={id}&usuarioId={usuarioId}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<ApiResponse?> EditarIncidenteAsync(
        int id,
        int usuarioId,
        string tipoIncidente,
        string descripcion,
        string direccionReferencia,
        double latitud,
        double longitud,
        FileResult? imagen)
    {
        try
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(id.ToString()), "id");
            content.Add(new StringContent(usuarioId.ToString()), "usuarioId");
            content.Add(new StringContent(tipoIncidente), "tipoIncidente");
            content.Add(new StringContent(descripcion), "descripcion");
            content.Add(new StringContent(direccionReferencia), "direccionReferencia");
            content.Add(new StringContent(latitud.ToString(System.Globalization.CultureInfo.InvariantCulture)), "latitud");
            content.Add(new StringContent(longitud.ToString(System.Globalization.CultureInfo.InvariantCulture)), "longitud");

            if (imagen != null)
            {
                var stream = await imagen.OpenReadAsync();
                var fileContent = new StreamContent(stream);
                content.Add(fileContent, "imagenEvidencia", imagen.FileName);
            }

            var response = await _httpClient.PostAsync($"{BaseUrl}/Api/EditarIncidente", content);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ApiResponse>();
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<NotificacionModel>> ObtenerNotificacionesAsync(int usuarioId)
    {
        try
        {
            var lista = await _httpClient.GetFromJsonAsync<List<NotificacionModel>>(
                $"{BaseUrl}/Api/Notificaciones?usuarioId={usuarioId}");

            return lista ?? new List<NotificacionModel>();
        }
        catch
        {
            return new List<NotificacionModel>();
        }
    }

    public async Task<List<MapaModel>> ObtenerMapaDashboardAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<MapaModel>>
            (
                $"{BaseUrl}/Api/MapaDashboard"
            ) ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<ApiResponse?> EliminarIncidenteAsync(int id, int usuarioId)
    {
        try
        {
            var datos = new Dictionary<string, string>
        {
            { "id", id.ToString() },
            { "usuarioId", usuarioId.ToString() }
        };

            var response = await _httpClient.PostAsync(
                $"{BaseUrl}/Api/EliminarIncidente",
                new FormUrlEncodedContent(datos));

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ApiResponse>();
        }
        catch
        {
            return null;
        }
    }

    public async Task<AlertaSOSModel?> ObtenerMiSOSActivoAsync(int usuarioId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<AlertaSOSModel>(
                $"{BaseUrl}/Api/MiSOSActivo?usuarioId={usuarioId}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<ApiResponse?> FinalizarSOSAsync(int usuarioId, int alertaId)
    {
        try
        {
            var datos = new Dictionary<string, string>
            {
                { "usuarioId", usuarioId.ToString() },
                { "alertaId", alertaId.ToString() }
            };

            var response = await _httpClient.PostAsync($"{BaseUrl}/Api/FinalizarSOS", new FormUrlEncodedContent(datos));

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ApiResponse>();
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<AlertaSOSModel>> ObtenerAlertasRecientesAsync()
    {
        try
        {
            var lista = await _httpClient.GetFromJsonAsync<List<AlertaSOSModel>>(
                $"{BaseUrl}/Api/AlertasRecientes");

            return lista ?? new List<AlertaSOSModel>();
        }
        catch
        {
            return new List<AlertaSOSModel>();
        }
    }

    public async Task<AdminDashboardModel?> ObtenerAdminDashboardAsync(int usuarioId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<AdminDashboardModel>(
                $"{BaseUrl}/Api/AdminDashboard?usuarioId={usuarioId}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<UsuarioAdminModel>> ObtenerUsuariosAdminAsync(int usuarioId)
    {
        try
        {
            var lista = await _httpClient.GetFromJsonAsync<List<UsuarioAdminModel>>(
                $"{BaseUrl}/Api/UsuariosAdmin?usuarioId={usuarioId}");

            return lista ?? new List<UsuarioAdminModel>();
        }
        catch
        {
            return new List<UsuarioAdminModel>();
        }
    }

    public async Task<List<IncidenteModel>> ObtenerIncidentesAdminAsync(int usuarioId)
    {
        try
        {
            var lista = await _httpClient.GetFromJsonAsync<List<IncidenteModel>>(
                $"{BaseUrl}/Api/IncidentesAdmin?usuarioId={usuarioId}");

            return lista ?? new List<IncidenteModel>();
        }
        catch
        {
            return new List<IncidenteModel>();
        }
    }

    public async Task<List<AlertaSOSModel>> ObtenerAlertasSOSAdminAsync(int usuarioId)
    {
        try
        {
            var lista = await _httpClient.GetFromJsonAsync<List<AlertaSOSModel>>(
                $"{BaseUrl}/Api/AlertasSOSAdmin?usuarioId={usuarioId}");

            return lista ?? new List<AlertaSOSModel>();
        }
        catch
        {
            return new List<AlertaSOSModel>();
        }
    }
    public async Task<List<IncidenteModel>> ObtenerMisReportesAsync(int usuarioId)
    {
        try
        {
            var lista = await _httpClient.GetFromJsonAsync<List<IncidenteModel>>(
                $"{BaseUrl}/Api/MisReportes?usuarioId={usuarioId}");

            return lista ?? new List<IncidenteModel>();
        }
        catch
        {
            return new List<IncidenteModel>();
        }
    }
}

public class LoginResponse
{
    public bool ok { get; set; }
    public int id { get; set; }
    public string? nombre { get; set; }
    public string? correo { get; set; }
    public bool esAdmin { get; set; }
    public string? mensaje { get; set; }
}

public class ApiResponse
{
    public bool ok { get; set; }
    public string? mensaje { get; set; }
}

public class AsistenciaResponse
{
    public bool ok { get; set; }
    public string? respuesta { get; set; }
}
