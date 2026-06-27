using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SafeBarrio.MAUI.Models;

public class NotificacionModel
{
    public int id { get; set; }
    public string? titulo { get; set; }
    public string? mensaje { get; set; }
    public string? fecha { get; set; }
    public bool leida { get; set; }
}