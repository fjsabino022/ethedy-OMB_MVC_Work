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

      //  PRUEBA 1: probando change tracker
      //
          //  ProbarChangeTracker();

      //  PRUEBA 2: cambiar todos los perfiles que tiene un Usuario
      //
          //  CrearUsuarioCompleto();
          //  CambiarSoloPerfiles();

      //  PRUEBA 3: borramos un perfil, agregamos otro, cambiamos de Persona y un par de propiedades
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

      Console.WriteLine("Eliminando posible usuario remanente lsimpson");

      //  Me aseguro que el usuario no exista, solo para fines de prueba, no es un metodo que deba existir en la realidad
      serv.EliminarUsuario("lsimpson");

      //  Creo Persona, Usuario y le asigno al menos un Perfil porque de otra manera la validacion en SecurityServices
      //  fallaria
      //  primero chequeamos que la Persona no exista, si no existe la creamos
      Persona persona ;

      Console.WriteLine("Chequeando si existe Persona para Lista Simpson");
      persona = (from per in DB.Contexto.Personas
                      where per.Apellido == "Simpson" && per.Nombre == "Lisa"
                      select per).SingleOrDefault();

      if (persona == null)
      {
        Console.WriteLine("Creando Persona Lisa Simpson");
        persona = CrearPersonaInternal("Lisa", "Simpson", "lisa_simp@mensa.org", 8);
      }

      Console.WriteLine("Creando Usuario a partir de Persona");
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

      Persona nuevaPersona;

      //  primero chequeamos que la Persona no exista, si no existe la creamos
      nuevaPersona = (from per in DB.Contexto.Personas where per.Apellido == "Olsen" && per.Nombre == "Mona Penelope" 
                      select per).SingleOrDefault();
      
      if (nuevaPersona == null)
        nuevaPersona = CrearPersonaInternal("Mona Penelope", "Olsen", "mona@fugitivos.org", 78);

      user.Persona = nuevaPersona;

      user.EnforceExpiration = true;
      user.FechaExpiracionPassword = DateTime.Now.AddDays(120);
      
      serv.UpdateUsuario(user, "123-lisa");
    }

    /// <summary>
    /// No tocamos la DB, solo verificamos que EF hace el seguimiento de todos los objetos que se han modificado
    /// </summary>
    public static void ProbarChangeTracker()
    {
      Console.WriteLine("Probando conexion a base de datos y DbSet<Usuario>");

      Usuario user = (from usr in DB.Contexto.Usuarios where usr.Login == "mburns" select usr).SingleOrDefault();

      Usuario otroUser = (from usr in DB.Contexto.Usuarios where usr.Login == "hsimpson" select usr).SingleOrDefault();

      //  otroUser no vamos a tocarlo

      if (user != null)
      {
        Console.WriteLine("Conexion y DbSet OK");

        //  cambiamos algunos datos del Usuario
        user.Enabled = false;
        user.MustChangePass = true;

        //  cambiamos algunos datos de la Persona asociada
        user.Persona.Localidad = "Springfield";
        user.Persona.Telefono = "555-4125";

        Console.WriteLine("Probando ChangeTracker");
        Console.WriteLine("Resultado esperado: un objeto Usuario y un objeto Persona modificados. Un objeto Usuario sin modificar");
        Console.WriteLine("===========================================================================");
        //  Consultamos el change-tracker a ver que cambio...
        //  Iteramos cada entidad que es seguida por el change tracker
        //
        foreach (var entidad in DB.Contexto.ChangeTracker.Entries())
        {
          Console.WriteLine("Entidad de tipo: {0}\nEstado: {1}",
            entidad.Entity.GetType().FullName,
            entidad.State);

          //  mostramos una propiedad para identificar el objeto...
          if (entidad.Entity is Usuario)
            Console.WriteLine("Entidad tipo Usuario, propiedad Login = {0}", entidad.Property("Login").CurrentValue);
          else
            Console.WriteLine("Entidad tipo Persona, propiedad CorreoElectronico = {0}", entidad.Property("CorreoElectronico").CurrentValue);

          Console.WriteLine(new string('=', 80));
        }
      }
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
