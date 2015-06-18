using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entidades;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace TestEF
{
  class Program
  {
    static void Main(string[] args)
    {
      OMBContext ctx = new OMBContext();

      var per = from p in ctx.Usuarios select p;

      Usuario pp = per.FirstOrDefault();

      Persona xx = pp.Persona;
      Console.WriteLine(pp.Login);
    }
  }

  public class OMBContext : DbContext
  {
    public DbSet<Persona> Personas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    public OMBContext() : base("Server=CLUE;Database=OMB;Trusted_Connection=true") { }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Configurations.Add(new UsuarioConfiguration());
      modelBuilder.Configurations.Add(new PersonaConfiguration());
      modelBuilder.Configurations.Add(new PerfilConfiguration());
    }
  }

  public class PersonaConfiguration : EntityTypeConfiguration<Persona>
  {
    public PersonaConfiguration()
    {
      Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
    }
  }

  public class UsuarioConfiguration : EntityTypeConfiguration<Usuario>
  {
    public UsuarioConfiguration()
    {
      HasKey(usr => usr.Login);

      HasRequired(usr => usr.Persona).WithOptional().Map(configuration => configuration.MapKey("ID_Persona"));
      HasMany(usr => usr.Perfiles).WithMany().Map(x =>
      {
        x.ToTable("Usuarios_Perfiles");
        x.MapLeftKey("Login");
        x.MapRightKey("ID_Perfil");
      });
    }
  }

  public class PerfilConfiguration : EntityTypeConfiguration<Perfil>
  {
    public PerfilConfiguration()
    {
      ToTable("Perfiles");
      Property(x => x.ID).HasColumnName("ID_Perfil");
    }
  }
}
