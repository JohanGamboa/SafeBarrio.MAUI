using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SafeBarrio.MAUI.Models;

public class AdminDashboardModel
{
    public bool ok { get; set; }
    public int totalUsuarios { get; set; }
    public int totalIncidentes { get; set; }
    public int sosActivos { get; set; }
    public int sosResueltos { get; set; }

    public List<IncidenteModel>? ultimosIncidentes { get; set; }
    public List<AlertaSOSModel>? ultimasAlertas { get; set; }
}