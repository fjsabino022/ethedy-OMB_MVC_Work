using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Database;
using Entidades.Seguridad;

namespace Servicios
{
  /// <summary>
  /// Provee diferentes funciones de servicios de seguridad: login, cambio de password, encriptacion, auditoria
  /// </summary>
  public class SecurityServices
  {
    public SecurityServices()
    {

    }

    public Sesion Login(string uid, string pwd)
    {
      Usuario user;

      //  usamos exp-L para hallar el usuario que coincide con la info de login
      //
      user = DB.Usuarios.Find(u => u.Login == uid);

      if (user != null)
      {
        if (DB.LoginUsuario(user, pwd))
        {
          Sesion result = new Sesion(user);

          return result;
        }
      }
      return null;
    }
  }
}
