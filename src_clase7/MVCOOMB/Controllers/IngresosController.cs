using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Entidades;

namespace MvcOMB.Controllers
{
  public class IngresosController : Controller
  {
    //  Action Method para un nuevo libro en blanco (retorna una vista con los campos vacios...)
    public ActionResult NewLibro()
    {
      Libro nuevoLibro = new Libro();

      return View(nuevoLibro);
    }

    public ActionResult Agregar(Libro newLibro)
    {
      OMBContext ctx = DB.Contexto;

      if (string.IsNullOrEmpty(newLibro.ISBN13))
        ModelState.AddModelError("ISBN13", "El campo ISBN nuevo no puede dejarse vacio!!");

      if (ModelState.IsValid)
      {
        try
        {
          ctx.Libros.Add(newLibro);
          ctx.SaveChanges();
        }
        catch (Exception)
        {
          return new HttpUnauthorizedResult();
        }
        return View();
      }
      return View("NewLibro", newLibro);
    }
  }
}