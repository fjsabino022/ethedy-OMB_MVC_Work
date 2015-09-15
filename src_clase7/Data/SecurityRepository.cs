using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entidades;
using Infraestructura;

namespace Data
{
  /// <summary>
  /// Contiene todas las interacciones necesarias del submodulo de seguridad contra la DB (a traves de EF)
  /// </summary>
  public class SecurityRepository
  {
    private const string CMD_CHANGEPASS = @"CambiarPasswordForLogin @p0, @p1";

    private const string CMD_VALIDATEPASS = @"ValidarPasswordForLogin @p0, @p1";

    /// <summary>
    /// Obtiene el usuario determinado desde la DB con todas sus propiedades cargadas
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public Usuario GetUsuarioFromLogin(string login)
    {
      OMBContext ctx = DB.Contexto;
      Usuario result;

      var query = from u in ctx.Usuarios.Include("Persona").Include("Perfiles")
        where u.Login == login
        select u;

      result = query.SingleOrDefault();
      return result;
    }

    /// <summary>
    /// Ejecuta las acciones en el repositorio para crear un nuevo usuario
    /// La Password se setea dentro de la misma transaccion que crea el Usuario
    /// En caso de que la Persona asociada tambien sea nueva, se agregara automaticamente
    /// </summary>
    /// <param name="user"></param>
    /// <param name="pass"></param>
    public void CrearUsuario(Usuario user, string pass)
    {
      OMBContext ctx = DB.Contexto;

      using (var trans = ctx.Database.BeginTransaction())
      {
        try
        {
          ctx.Usuarios.Add(user);
          
          if (ctx.VerboseMode) 
            ctx.MostrarCambios("Cambios en CT para CrearUsuario(user, pass)");

          ctx.SaveChanges();

          var query = CrearQueryCambioPassword(user.Login, pass);

          int resultado = query.FirstOrDefault();

          if (resultado != 1)
            throw new OMBDataPersistenceError("Error al intentar setear la password para el usuario", null);

          trans.Commit();
        }
        catch (Exception ex)
        {
          trans.Rollback();
          throw new OMBDataPersistenceError("Error al intentar agregar un nuevo Usuario", ex);
        }
      }
    }

    /// <summary>
    /// Realiza la tarea concreta de eliminar el Usuario y los perfiles que tiene asociado
    /// Se da la opcion para eliminar la Persona asociada al usuario tambien
    /// </summary>
    /// <param name="login"></param>
    /// <param name="eliminarPersona"></param>
    public void EliminarUsuario(string login, bool eliminarPersona)
    {
      OMBContext ctx = DB.Contexto;
      Usuario user = (from u in ctx.Usuarios.Include("Persona").Include("Perfiles")
                      where u.Login == login
                      select u).SingleOrDefault();

      if (user != null)
      {
        //  Si lo hago al reves (primero Remove(user)) cuando quiero hacer Remove(user.Persona) resulta que ya la 
        //  nav-prop esta seteada en null y por supuesto da error...
        if (eliminarPersona)
          ctx.Personas.Remove(user.Persona);

        ctx.Usuarios.Remove(user);

        if (ctx.VerboseMode)
          ctx.MostrarCambios("Cambios en CT para EliminarUsuario(login, bool)");

        ctx.SaveChanges();
      }
    }

    /// <summary>
    /// Si el usuario fue modificado y ademas quiero cambiar la password, ejecuta ambas operaciones 
    /// dentro de una transaccion
    /// De otro modo, hace una de las dos normalmente (sin transaccion)
    /// </summary>
    /// <param name="user"></param>
    /// <param name="newPass"></param>
    public void ModificarUsuario(Usuario user, string newPass = null)
    {
      OMBContext ctx = DB.Contexto;
      bool fUserChanged;

      fUserChanged = ctx.ChangeTracker.HasChanges();
      if (fUserChanged && newPass != null)
      {
        using (var trans = ctx.Database.BeginTransaction())
        {
          try
          {
            if (ctx.VerboseMode)
              ctx.MostrarCambios("Cambios en CT para ModificarUsuario(user, pass)");

            ctx.SaveChanges();

            var query = CrearQueryCambioPassword(user.Login, newPass);

            int resultado = query.FirstOrDefault();

            if (resultado != 1)
              throw new OMBDataPersistenceError("Error al intentar setear la password para el usuario", null);

            trans.Commit();
          }
          catch (Exception ex)
          {
            trans.Rollback();
            throw new OMBDataPersistenceError("Error al intentar actualizar el usuario o la password", ex);
          }
        }
      }
      else
      {
        if (fUserChanged)
        {
          try
          {
            ctx.SaveChanges();
          }
          catch (Exception ex)
          {
            throw new OMBDataPersistenceError("Error al intentar actualizar el Usuario", ex);
          }
        }
        else
        {
          if (newPass != null)
          {
            try
            {
              var query = CrearQueryCambioPassword(user.Login, newPass);

              int resultado = query.FirstOrDefault();

              if (resultado != 1)
                throw new OMBDataPersistenceError("Error al intentar setear la password para el usuario", null);
            }
            catch (Exception ex)
            {
              throw new OMBDataPersistenceError("Error al intentar actualizar la password", ex);
            }
          }
        }
      }
    }

    /// <summary>
    /// Verifica que la combinacion de Usuario/Password sea la correcta
    /// Actualiza la informacion de LastLogin del Usuario
    /// </summary>
    /// <param name="usr"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    public bool LoginUsuario(Usuario usr, string pass)
    {
      OMBContext ctx = DB.Contexto;

      var query = CrearQueryValidarPassword(usr.Login, pass);

      int resultado = query.FirstOrDefault();

      return (resultado == 1);
    }


    private DbRawSqlQuery<int> CrearQueryCambioPassword(string login, string pass)
    {
      return DB.Contexto.Database.SqlQuery<int>(CMD_CHANGEPASS, login, pass);
    }

    private DbRawSqlQuery<int> CrearQueryValidarPassword(string login, string pass)
    {
      return DB.Contexto.Database.SqlQuery<int>(CMD_VALIDATEPASS, login, pass);
    }
  }
}
