using SafeBarrio.MAUI.Pages;

namespace SafeBarrio.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(RegistroPage), typeof(RegistroPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        Routing.RegisterRoute(nameof(SoportePage), typeof(SoportePage));
        Routing.RegisterRoute(nameof(SOSPage), typeof(SOSPage));
        Routing.RegisterRoute(nameof(MisReportesPage), typeof(MisReportesPage));
        Routing.RegisterRoute(nameof(ReportarIncidentePage), typeof(ReportarIncidentePage));
        Routing.RegisterRoute(nameof(AsistenciaPage), typeof(AsistenciaPage));
        Routing.RegisterRoute(nameof(EditarIncidentePage), typeof(EditarIncidentePage));
        Routing.RegisterRoute(nameof(NotificacionesPage), typeof(NotificacionesPage));
        Routing.RegisterRoute(nameof(PerfilPage), typeof(PerfilPage));
        Routing.RegisterRoute(nameof(AdminDashboardPage), typeof(AdminDashboardPage));
        Routing.RegisterRoute(nameof(UsuariosAdminPage), typeof(UsuariosAdminPage));
        Routing.RegisterRoute(nameof(IncidentesAdminPage), typeof(IncidentesAdminPage));
        Routing.RegisterRoute(nameof(AlertasSOSAdminPage), typeof(AlertasSOSAdminPage));
    }
}