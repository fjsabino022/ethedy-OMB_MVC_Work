using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
  public class Libro
  {
    public string ISBN13 { get; set; }

    public string ISBN10 { get; set; }

    public string Editorial { get; set; }

    public string Titulo { get; set; }

    public string Subtitulo { get; set; }

    /// <summary>
    /// Puede ser Primera, Primera Revisada, etc...por eso no pongo numerico...
    /// </summary>
    public string Edicion { get; set; }

    /// <summary>
    /// Obviamente hay que pasarla a una coleccion que referencie a otra entidad...
    /// </summary>
    public string Autores { get; set; }

    public decimal? Precio { get; set; }

    public string PathImagen { get; set; }
  }
}
