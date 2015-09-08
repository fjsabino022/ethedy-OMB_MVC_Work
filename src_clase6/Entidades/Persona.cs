
using System;

namespace Entidades
{
  /// <summary>
  /// Una Persona puede ser un cliente, un empleado, un proveedor o cualquier participante del sistema
  /// </summary>
  public class Persona
  {
    public Guid ID { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Direccion { get; set; }
    public string Localidad { get; set; }
    public string Provincia { get; set; }
    public string CodigoPostal { get; set; }
    public string CorreoElectronico { get; set; }
    public string Telefono { get; set; }
    public DateTime FechaNacimiento { get; set; }

    //  TODO hacer que Telefono, CorreoElectronico, etc sean entidades ContactInfo ya que la persona puede tener mas de uno de cada uno
    //  TODO Direccion podria ser una estructura compleja, tambien la persona podria tener mas de una (address list de Amazon)
  }
}
