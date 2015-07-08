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
      SecurityServices serv = new SecurityServices();
      Usuario user;

      //  serv.EliminarUsuario("lsimpson");

      user = serv.GetUsuarioFromLogin("lsimpson");

      //OMBContext Contexto = new OMBContext();

      //Contexto.Configuration.LazyLoadingEnabled = true;
      //Contexto.Configuration.AutoDetectChangesEnabled = true;

      //  Persona persona = CrearPersona("Bart", "Simpson", "elbarto@krustyclub.com", 10);
      //  Usuario user = CrearUsuario("lsimpson", persona);


      var perfiles = from p in DB.Contexto.Perfiles where p.ID==2 || p.ID==3 select p;

      Persona abuelo = (from per in DB.Contexto.Personas where per.Nombre == "Lisa" select per).Single();

      //  user.Perfiles = new HashSet<Perfil>(perfiles);

      //  Con esto funciona...
      //  user.MustChangePass = true;
      //  user.FechaLastLogin = DateTime.Now;

      //  vamos a probar con los perfiles
      //  primero quitamos un perfil de los que ya tiene
      //  user.Perfiles.RemoveWhere(p => p.ID == 2);
      //user.Perfiles.Clear();

      //  ahora agregamos el otro perfil
      //    user.Perfiles.Add(perfiles.Single());
      //user.Perfiles.UnionWith(perfiles);

      user.Persona = abuelo;

      //user.FechaLastLogin = DateTime.Now;
      //user.EnforceStrong = true;
      try
      {
        //Contexto.SaveChanges();
        //  serv.CrearUsuario(user, "lisa-123");
        serv.UpdateUsuario(user);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    public static Persona CrearPersona(string nombre, string apellido, string mail, int edad)
    {
      return new Persona()
      {
        Apellido = apellido,
        Nombre = nombre,
        CorreoElectronico = mail,
        FechaNacimiento = DateTime.Now.AddYears(-edad)
      };
    }

    public static Usuario CrearUsuario(string login, Persona persona)
    {
      return new Usuario()
      {
        Login = login,
        FechaExpiracionPassword = DateTime.Now.AddDays(30),
        Persona = persona
      };
    }
  }
}
