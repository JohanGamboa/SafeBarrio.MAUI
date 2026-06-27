using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SafeBarrio.MAUI.Models;

public class UsuarioAdminModel
{
    public int id { get; set; }
    public string? nombre { get; set; }
    public string? correo { get; set; }
    public string? telefono { get; set; }
    public string? direccion { get; set; }
    public bool esAdmin { get; set; }
}