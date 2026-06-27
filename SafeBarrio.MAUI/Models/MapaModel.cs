using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SafeBarrio.MAUI.Models;

public class MapaModel
{
    public string? tipo { get; set; }

    public string? titulo { get; set; }

    public string? descripcion { get; set; }

    public double latitud { get; set; }

    public double longitud { get; set; }

    public string? estado { get; set; }
}