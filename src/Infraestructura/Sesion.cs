using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entidades;
using Entidades.Seguridad;

namespace Servicios
{
  /// <summary>
  /// Mantiene durante el transcurso de la aplicacion toda la informacion necesaria asociada con el usuario conectado
  /// </summary>
  public class Sesion
  {
    public Usuario UsuarioConectado { get; private set; }

    public string FullName
    {
      get
      {
        return string.Format("{0} {1}",
          UsuarioConectado.Persona.Nombre,
          UsuarioConectado.Persona.Apellido);
      }
    }

    public DateTime FechaExpiracion
    {
      get { return UsuarioConectado.FechaExpiracionPassword; }
    }

    public Sesion(Usuario usr)
    {
      UsuarioConectado = usr;
    }
  }
}
