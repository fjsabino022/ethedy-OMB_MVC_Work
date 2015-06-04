using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Database;
using Entidades;

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

    public Usuario Login(Usuario usr, string pwd)
    {
      Usuario user;

      //  usamos exp-l para comprobar que el usuario exista (y obtenerlo)
      //
      user = DB.Usuarios.Find(u => u.Login == usr.Login);

      if (user != null)
      {
        if (DB.LoginUsuario(user, pwd)) //  ojo con lo que le paso!!!
          return user;
      }
      return null;
    }

    public Usuario Login(string uid, string pwd)
    {
      Usuario user;

      //  usamos exp-L para hallar el usuario que coincide con la info de login
      //
      user = DB.Usuarios.Find(u => u.Login == uid);

      if (user != null)
      {
        if (DB.LoginUsuario(user, pwd))
        {
          //  Sesion result = new Sesion(user);
          return user;
        }
      }
      return null;
    }

    //  TODO chequear que el perfil corresponde al usuario??
    public Sesion CrearSesion(Usuario usr, Perfil perfil)
    {
      Sesion result = new Sesion(usr, perfil);

      return result;
    }
  }
}
