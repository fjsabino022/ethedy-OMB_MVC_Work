using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Data;
using Entidades;
using Infraestructura;

namespace Servicios
{
  /// <summary>
  /// Provee diferentes funciones de servicios de seguridad: login, cambio de password, encriptacion, auditoria
  /// </summary>
  public class SecurityServices
  {
    public SecurityServices() { }

    public Usuario Login(string uid, string pwd)
    {
      Usuario user;

      user = GetUsuarioFromLogin(uid);

      if (user != null)
      {
        if (user.Enabled)
        {
          SecurityRepository repo = new SecurityRepository();

          if (repo.LoginUsuario(user, pwd))
          {
            //  Sesion result = new Sesion(user);
            user.FechaLastLogin = DateTime.Now;
            repo.ModificarUsuario(user);

            return user;
          }
        }
        else
          throw new OMBSecurityException("El Usuario ha sido bloqueado por el administrador....");
      }
      return null;
    }

    /// <summary>
    /// Obtiene el usuario con todos sus campos cargados
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public Usuario GetUsuarioFromLogin(string login)
    {
      SecurityRepository repo = new SecurityRepository();

      return repo.GetUsuarioFromLogin(login);
    }

    /// <summary>
    /// Genera un nuevo Usuario en el sistema, seteando para el mismo la password que se pasa como argumento
    /// Valida que las reglas de negocio para el Usuario se cumplan, en este caso, que se le haya asignado al 
    /// menos un Perfil ya que es obligatorio
    /// </summary>
    /// <param name="newUser"></param>
    /// <param name="newPass"></param>
    /// <returns></returns>
    public void CrearUsuario(Usuario newUser, string newPass)
    {
      SecurityRepository repo = new SecurityRepository();

      if (newUser.Perfiles != null && newUser.Perfiles.Count >= 1)
      {
        repo.CrearUsuario(newUser, newPass);
      }
      else
        throw new OMBBusinessRuleException("Un Usuario debe tener al menos un Perfil asociado");
    }

    /// <summary>
    /// Este metodo podriamos decir que esta de mas ya que el contexto sabe exactamente lo que esta ocurriento
    /// pero tengo que validar los Perfiles
    /// </summary>
    /// <param name="user"></param>
    /// <param name="pass"></param>
    public void UpdateUsuario(Usuario user, string pass = null)
    {
      SecurityRepository repo = new SecurityRepository();

      if (user.Perfiles != null && user.Perfiles.Count >= 1)
      {
        repo.ModificarUsuario(user, pass);
      }
      else
        throw new OMBBusinessRuleException("Un Usuario debe tener al menos un Perfil asociado");
    }

    /// <summary>
    /// Este es un experimento...en teoria no seria necesario eliminar un Usuario, a lo sumo deberiamos bloquearlo
    /// para que no pueda ingresar, pero eliminarlo tambien implicaria quitar todas las referencias desde otras
    /// posibles tablas que podrian existir
    /// Ademas borramos todos los datos, Usuario, Perfiles... Persona opcionalmente
    /// </summary>
    public void EliminarUsuario(string login)
    {
      SecurityRepository repo = new SecurityRepository();

      repo.EliminarUsuario(login, true);
    }

    //  TODO chequear que el perfil corresponde al usuario??
    public Sesion CrearSesion(Usuario usr, Perfil perfil)
    {
      Sesion result = new Sesion(usr, perfil);

      Context.Current.Sesion = result;
      return result;
    }

    public void CerrarSesion()
    {
      Context.Current.Sesion.Logout();
      Context.Current.Sesion = null;
    }
  }
}
