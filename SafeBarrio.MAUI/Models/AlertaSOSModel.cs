using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SafeBarrio.MAUI.Models;

public class AlertaSOSModel
{
    public int id { get; set; }
    public double latitud { get; set; }
    public double longitud { get; set; }
    public string? fechaAlerta { get; set; }
    public string? estado { get; set; }
    public string? mensaje { get; set; }
}
