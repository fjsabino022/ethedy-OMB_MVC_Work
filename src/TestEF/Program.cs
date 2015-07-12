using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Entidades;
using Microsoft.SqlServer.Server;
using Servicios;

namespace TestEF
{
  class Program
  {
    static void Main(string[] args)
    {
      //  Comentar o descomentar segun las pruebas que se quieran hacer

      //  PRUEBA 1: cambiar todos los perfiles que tiene un Usuario
      //
      CrearUsuarioCompleto();
      CambiarSoloPerfiles();

      //  PRUEBA 2: borramos un perfil, agregamos otro, cambiamos de Persona y un par de propiedades
      //
          //  CrearUsuarioCompleto();
          //  CambiarCompleto();


      Console.WriteLine();
      Console.WriteLine("Presionar [Enter] para finalizar");
      Console.ReadLine();
    }

    public static void CrearUsuarioCompleto()
    {
      SecurityServices serv = new SecurityServices();
      Usuario user;

      //  Me aseguro que el usuario no exista, solo para fines de prueba, no es un metodo que deba existir en la realidad
      serv.EliminarUsuario("lsimpson");

      //  Creo Persona, Usuario y le asigno al menos un Perfil porque de otra manera la validacion en SecurityServices
      //  fallaria
      Persona persona = CrearPersonaInternal("Lisa", "Simpson", "lisa_simp@mensa.org", 8);
      user = CrearUsuarioInternal("lsimpson", persona);

      //  lo saco directamente del OMBContext pero deberia hacerlo de SecServices (pensar que se utilizaria tambien
      //  en la UI donde asignamos o quitamos perfiles a un usuario)
      //
      var perfiles = from p in DB.Contexto.Perfiles where p.ID == 2 || p.ID == 3 select p;
      user.Perfiles = new HashSet<Perfil>(perfiles);

      //  actualizamos el usuario en la DB
      serv.CrearUsuario(user, "lisa-123");
    }

    /// <summary>
    /// Tomamos un Usuario, borramos los perfiles que tiene y agregamos los que no tenia
    /// Visualizar en SQL Profiler: se veran las dos instrucciones delete y los 2 insert en Usuarios_Perfiles
    /// [[IMPORTANTE]]
    /// Antes de probar este metodo, asegurarse de haber llamado a CrearUsuarioCompleto() ya que ahi es donde
    /// colocamos los perfiles 
    /// </summary>
    public static void CambiarSoloPerfiles()
    {
      SecurityServices serv = new SecurityServices();
      Usuario user;

      //  Seguramente no se hace un viaje a la DB, este objeto ya esta en memoria local...
      user = serv.GetUsuarioFromLogin("lsimpson");

      var perfiles = from p in DB.Contexto.Perfiles where p.ID == 1 || p.ID == 4 select p;

      user.Perfiles.Clear();
      user.Perfiles.UnionWith(perfiles);

      serv.UpdateUsuario(user);
    }

    /// <summary>
    /// Tomamos un Usuario, cambiamos la Persona asociada al mismo, algunas propiedades escalares, eliminamos un perfil
    /// y agregamos otro. Tambien modificamos la contraseña
    /// Luego de probar este metodo se puede acceder a la app principal y ver que pasa, obviamente ingresando con la
    /// nueva password
    /// [[IMPORTANTE]]
    /// Antes de probar este metodo, asegurarse de haber llamado a CrearUsuarioCompleto()
    /// </summary>
    public static void CambiarCompleto()
    {
      SecurityServices serv = new SecurityServices();
      Usuario user;

      user = serv.GetUsuarioFromLogin("lsimpson");

      var perfiles = from p in DB.Contexto.Perfiles where p.ID == 4 select p;

      user.Perfiles.RemoveWhere(perf => perf.ID == 2);
      user.Perfiles.Add(perfiles.Single());

      Persona abuelo = (from per in DB.Contexto.Personas where per.Nombre == "Abraham" select per).Single();
      user.Persona = abuelo;

      user.EnforceExpiration = true;
      user.FechaExpiracionPassword = DateTime.Now.AddDays(120);
      
      serv.UpdateUsuario(user, "123-lisa");
    }

    public static Persona CrearPersonaInternal(string nombre, string apellido, string mail, int edad)
    {
      return new Persona()
      {
        Apellido = apellido,
        Nombre = nombre,
        CorreoElectronico = mail,
        FechaNacimiento = DateTime.Now.AddYears(-edad)
      };
    }

    public static Usuario CrearUsuarioInternal(string login, Persona persona)
    {
      return new Usuario()
      {
        Login = login,
        FechaExpiracionPassword = DateTime.Now.AddDays(30),
        Enabled = true,
        Persona = persona
      };
    }
  }
}
