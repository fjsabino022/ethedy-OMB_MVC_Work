using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data;
using Entidades;

namespace MvcOMB.Models
{
  public class ListadosViewModel
  {
    public IList<Libro> GetAlllibros()
    {
      //  where stock > 1 por ejemplo...
      return DB.Contexto.Libros.ToList();  
    }
  }
}