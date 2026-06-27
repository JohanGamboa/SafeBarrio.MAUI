using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SafeBarrio.MAUI.Models;


public class UsuarioModel
{
    public int id { get; set; }
    public string? nombre { get; set; }
    public string? apellido { get; set; }
    public string? correo { get; set; }
    public string? telefono { get; set; }
    public string? ubicacion { get; set; }
    public bool esAdmin { get; set; }
}