using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeBarrio.MAUI.Models;

namespace SafeBarrio.MAUI.Models;

public class IncidenteModel
{
    public int id { get; set; }

    public string? tipoIncidente { get; set; }

    public string? descripcion { get; set; }

    public string? direccionReferencia { get; set; }

    public string? estado { get; set; }

    public string? fechaReporte { get; set; }

    public string? imagenRuta { get; set; }

    public double latitud { get; set; }

    public double longitud { get; set; }
}