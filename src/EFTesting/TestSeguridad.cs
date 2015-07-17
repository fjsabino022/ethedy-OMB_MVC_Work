using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Data;
using Entidades;
using System.Linq;
using Servicios;
using Infraestructura;

namespace EFTesting
{
  /// <summary>
  /// Unit Test para las clases de Seguridad y su interaccion con EF
  /// </summary>
  [TestClass]
  public class TestSeguridad
  {
    private OMBContext Contexto { get; set; }
    
    private Persona _newPersona;        //  la persona que fue creada
    private Usuario _newUser;           //  el usuario que fue creado

    public TestSeguridad()
    {
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    //  [ClassCleanup]
    //  public static void LimpiarTodo() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    // Este metodo se invoca antes de cada llamada a un TestMethod
    //
    //  Tener en cuenta que en si en los test uso mas Usuarios deberia limpiarlos aca...
    [TestInitialize()]
    public void IniciarTest()
    {
      SecurityServices serv = new SecurityServices();
      
      serv.EliminarUsuario("lsimpson");

      //  observar que para cada test creo un contexto fresh....sin entidades residuales
      //  este contexto es diferente del que usa SecurityServices...
      Contexto = new OMBContext();

      //  siempre pongo el lazy load en false, la mayoria de las pruebas lo usa asi
      Contexto.Configuration.LazyLoadingEnabled = false;
      Contexto.Configuration.AutoDetectChangesEnabled = false;
    }

    [TestMethod()]
    [TestCategory("Eager Loading")]
    [Description(@"Obtiene un Usuario desde la DB y verifica que no se cargan las propiedades de navegacion. 
      Luego cuando se fuerza el eager loading las propiedades estan cargadas")]
    public void Comprobar_Eager_Loading_Persona_y_Perfiles()
    {
      Usuario user = (from u in Contexto.Usuarios where u.Login == "mbouvier" select u).FirstOrDefault();

      var entidad = Contexto.Entry(user);

      Assert.IsFalse(entidad.Reference(x => x.Persona).IsLoaded, "La propiedad Persona no deberia estar cargada");
      Assert.IsFalse(entidad.Collection(x => x.Perfiles).IsLoaded, "La propiedad Perfiles no deberia estar cargada");

      user = (from u in Contexto.Usuarios.Include("Perfiles").Include("Persona") where u.Login == "mbouvier" select u).FirstOrDefault();

      entidad = Contexto.Entry<Usuario>(user);

      Assert.IsTrue(entidad.Reference(x => x.Persona).IsLoaded, "La propiedad Persona deberia estar pre-cargada");
      Assert.IsTrue(entidad.Collection(x => x.Perfiles).IsLoaded, "La propiedad Perfiles deberia estar pre-cargada");

      Assert.AreEqual(user.Persona.Apellido, "Bouvier");
      Assert.AreEqual(user.Perfiles.Count, 2);
    }

    [TestMethod]
    [TestCategory("Explicit Loading")]
    [Description(@"Obtiene un Usuario desde la DB, verifica que la navprop Persona no esta cargada, luego la obtiene explicitamente
                   sin tener que efectuar nuevamente la consulta")]
    public void Comprobar_Explicit_Loading_Persona()
    {
      Usuario user = (from u in Contexto.Usuarios where u.Login == "mbouvier" select u).FirstOrDefault();

      var entidad = Contexto.Entry(user);

      Assert.IsFalse(entidad.Reference(x => x.Persona).IsLoaded, "La propiedad Persona no deberia estar cargada");
      
      entidad.Reference(x => x.Persona).Load();

      Assert.IsTrue(entidad.Reference(x => x.Persona).IsLoaded, "La propiedad Persona debe estar cargada");
      Assert.AreEqual(user.Persona.Apellido, "Bouvier");
    }

    [TestMethod]
    [TestCategory("Explicit Loading")]
    [Description(@"Obtiene un Usuario desde la DB, verifica que la navprop Perfiles no esta cargada, luego la obtiene explicitamente
                   sin tener que efectuar nuevamente la consulta")]
    public void Comprobar_Explicit_Loading_Perfiles()
    {
      Usuario user = (from u in Contexto.Usuarios where u.Login == "mbouvier" select u).FirstOrDefault();

      var entidad = Contexto.Entry(user);

      Assert.IsFalse(entidad.Collection(x => x.Perfiles).IsLoaded, "La propiedad Perfiles no deberia estar cargada");

      entidad.Collection(x => x.Perfiles).Load();

      Assert.IsTrue(entidad.Collection(x => x.Perfiles).IsLoaded, "La propiedad Perfiles debe estar cargada");
      Assert.AreEqual(user.Perfiles.Count, 2);
    }

    [TestMethod]
    [TestCategory("Lazy Loading")]
    [Description(@"Carga Usuario desde la DB, comprobando que no se cargaron las propiedades de navegacion. Luego sin hacer otra cosa
                   que invocar dichas propiedades, comprobar que se cargan en el momento")]
    public void Comprobar_Lazy_Loading()
    {
      Contexto.Configuration.LazyLoadingEnabled = true;

      Usuario user = (from u in Contexto.Usuarios where u.Login == "mbouvier" select u).FirstOrDefault();

      var entidad = Contexto.Entry(user);

      Assert.IsFalse(entidad.Reference(x => x.Persona).IsLoaded, "La propiedad Persona no deberia cargarse hasta que se necesite");
      Assert.IsFalse(entidad.Collection(x => x.Perfiles).IsLoaded, "La propiedad Perfiles no deberia cargarse hasta que se necesite");

      Assert.AreEqual(user.Persona.Apellido, "Bouvier", "La propiedad Persona deberia cargarse en el momento de invocarse");
      Assert.AreEqual(user.Perfiles.Count, 2, "La propiedad Perfiles deberia cargarse en el momento de invocarse");

      //  no cambia nada, el tipo de Perfiles es List<Perfil>
      Debug.WriteLine(user.Perfiles.GetType().FullName);
    }

    [TestMethod]
    [TestCategory("Change Tracker")]
    [Description("Permite verificar que si se deshabilita el tracking de cambios hay que llamar a DetectChanges() para que el Contexto se entere de los mismos")]
    public void Comprobar_Auto_Detect_Changes_Disabled()
    {
      Contexto.Configuration.LazyLoadingEnabled = true;

      Usuario user = (from u in Contexto.Usuarios where u.Login == "mbouvier" select u).FirstOrDefault();

      user.FechaLastLogin = DateTime.Now;
      user.Persona.CodigoPostal = "S2000COK";

      Assert.IsFalse(Contexto.ChangeTracker.HasChanges(), "Con change tracking disabled el contexto no tiene que detectar modificaciones");
      Contexto.MostrarCambios("Antes de llamar a DetectChanges()");

      Contexto.ChangeTracker.DetectChanges();

      Assert.IsTrue(Contexto.ChangeTracker.HasChanges(), "Luego de llamar a DetectChanges tienen que verse los cambios realizados");
      Contexto.MostrarCambios("Luego de llamar a DetectChanges()");
    }

    [TestMethod]
    [TestCategory("Integridad")]
    [Description("Permite verificar que un Usuario existente no puede guardarse si no tiene una persona asociada")]
    [ExpectedException(typeof(DbUpdateException), "No se deberia poder guardar un Usuario con la propiedad Persona en null")]
    public void Comprobar_Relacion_Usuario_Existente_Persona()
    {
      Contexto.Configuration.LazyLoadingEnabled = true;
      Contexto.Configuration.AutoDetectChangesEnabled = true;

      Usuario user = (from u in Contexto.Usuarios where u.Login == "mbouvier" select u).Single();

      Persona p = user.Persona;   //  tengo que traer la propiedad porque si no es como si no existiera por el lazy-loading

      user.Persona = null;
      Contexto.MostrarCambios();
      Contexto.SaveChanges();
      /*
          An error occurred while saving entities that do not expose foreign key properties for their relationships. 
          The EntityEntries property will return null because a single entity cannot be identified as the source of the exception. 
          Handling of exceptions while saving can be made easier by exposing foreign key properties in your entity types. See the 
          InnerException for details.
       
          Inner Excepcion:
          A relationship from the 'Usuario_Persona' AssociationSet is in the 'Deleted' state. Given multiplicity constraints, a 
          corresponding 'Usuario_Persona_Source' must also in the 'Deleted' state. 
      */
    }

    [TestMethod]
    [TestCategory("Integridad")]
    [Description("Permite verificar que un Usuario nuevo no puede crearse si no tiene una persona asociada")]
    [ExpectedException(typeof(DbUpdateException), "No se deberia poder guardar un Usuario nuevo con la propiedad Persona en null")]
    public void Comprobar_Relacion_Usuario_Nuevo_Persona()
    {
      Contexto.Configuration.LazyLoadingEnabled = true;
      Contexto.Configuration.AutoDetectChangesEnabled = true;

      Usuario user = new Usuario()
      {
        Login = "lsimpson",
        FechaExpiracionPassword = DateTime.Now.AddDays(30),
        Persona = null
      };

      Contexto.Usuarios.Add(user);
      Contexto.MostrarCambios();
      Contexto.SaveChanges();
    }

    [TestMethod]
    [Description("Permite verificar la creacion exitosa de una nueva Persona")]
    public void Crear_Nueva_Persona()
    {
      Contexto.Configuration.LazyLoadingEnabled = true;
      Contexto.Configuration.AutoDetectChangesEnabled = true;

      _newPersona = CrearLisa();

      Contexto.Personas.Add(_newPersona);
      Contexto.MostrarCambios();
      Contexto.SaveChanges();

      Assert.AreNotEqual(default(Guid), _newPersona.ID, "Cuando se crea una nueva Persona su ID en formato GUID deberia tenerse en el modelo");
      Console.WriteLine("Se genero la Persona con el siguiente GUID: {0}", _newPersona.ID);

      LimpiarPersona(_newPersona);
    }

    [TestMethod]
    [Description("Permite chequear la regla de negocio que establece que un Usuario debe tener al menos un Perfil")]
    [ExpectedException(typeof(OMBBusinessRuleException), 
        "No se debe permitir la creacion de un Usuario sin al menos un Perfil asociado")]
    public void Comprobar_Que_Usuario_Nuevo_Necesita_Perfiles()
    {
      SecurityServices serv = new SecurityServices();

      _newPersona = CrearLisa();

      _newUser = new Usuario()
      {
        Login = "lsimpson",
        FechaExpiracionPassword = DateTime.Now.AddDays(30),
        Persona = _newPersona,
        Perfiles = new HashSet<Perfil>()
      };

      serv.CrearUsuario(_newUser, "temporal-123");
    }

    [TestMethod]
    [Description("Permite crear un Usuario completo con Persona asociada y Perfiles")]
    public void Comprobar_Creacion_Nuevo_Usuario()
    {
      SecurityServices serv = new SecurityServices();
      var perfiles = from perf in DB.Contexto.Perfiles.Take(2) select perf;

      _newPersona = CrearLisa();

      _newUser = new Usuario()
      {
        Login = "lsimpson",
        FechaExpiracionPassword = DateTime.Now.AddDays(30),
        Persona = _newPersona,
        Perfiles = new HashSet<Perfil>(perfiles)
      };

      serv.CrearUsuario(_newUser, "temporal-123");

      Assert.AreNotEqual(default(Guid), _newPersona.ID, 
        "Cuando se crea una nueva Persona su ID en formato GUID deberia tenerse en el modelo");
    }

    private void LimpiarPersona(Persona toDelete)
    {
      if (toDelete != null)
      {
        if (Contexto.Personas.Find(toDelete.ID) != null)
        {
          Contexto.Personas.Remove(toDelete);
          Contexto.MostrarCambios("Cambios para dejar la DB como ante de empezar las pruebas");
          Contexto.SaveChanges();
        }
      }
    }

    private void LimpiarUsuario(Usuario toDelete)
    {
      if (toDelete != null)
      {
        SecurityServices serv = new SecurityServices();

        //  tambien elimina la Persona 
        serv.EliminarUsuario(toDelete.Login);
      }
    }
    

    private Persona CrearLisa()
    {
      return new Persona()
      {
        Apellido = "Simpson",
        Nombre = "Lisa",
        CorreoElectronico = "lisa_simp@mensa.org",
        FechaNacimiento = new DateTime(1982, 8, 8)
      };
    }
  }
}
