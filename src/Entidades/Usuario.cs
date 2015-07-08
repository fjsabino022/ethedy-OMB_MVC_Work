
using System;
using System.Collections.Generic;

namespace Entidades
{
  /// <summary>
  /// Representa a una Persona que puede conectarse al sistema y puede interactuar con el mismo
  /// </summary>
  public class Usuario
  {
    /// <summary>
    /// Representa el ID unico de usuario dentro del sistema
    /// El ingreso al mismo debe realizarse con este identificador
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Si corresponde, la fecha en que expira la password del usuario
    /// </summary>
    public DateTime? FechaExpiracionPassword { get; set; }

    /// <summary>
    /// Fecha/Hora de la ultima conexion exitosa del usuario
    /// </summary>
    public DateTime? FechaLastLogin { get; set; }

    /// <summary>
    /// Indica si el usuario debe cambiar su contraseña la proxima vez que ingrese
    /// </summary>
    public bool? MustChangePass { get; set; }

    /// <summary>
    /// Indica si se debe obligar al usuario a cambiar su contraseña luego de un lapso de tiempo establecido por el sistema
    /// </summary>
    public bool? EnforceExpiration { get; set; }

    /// <summary>
    /// Indica si se la password del usuario tiene que cumplir con las restricciones de seguridad establecidas por el sistema
    /// </summary>
    public bool? EnforceStrong { get; set; }

    /// <summary>
    /// Indica si el usuario esta habilitado para ingresar al sistema
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Referencia a la persona que esta representando el usuario
    /// </summary>
    public virtual Persona Persona { get; set; }

    /// <summary>
    /// Lista de Perfiles que posee un Usuario
    /// Cuando decide EF que tipo de coleccion debe ser, elige HashSet que me parece que esta perfecto...
    /// </summary>
    public virtual HashSet<Perfil> Perfiles { get; set; }

    //  TODO agregar propiedad de ultimo login NO EXITOSO
    //  TODO agregar propiedad imagen del usuario (no lo ponemos en Persona, solamente usuarios del sistema que tienen oportunidad de mostrarla)
    //  
  }
}
