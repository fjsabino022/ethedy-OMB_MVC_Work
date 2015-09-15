using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Entidades;
using Data;


namespace MvcOMB.Controllers
{
  public class IngresosController : Controller
  {
    // GET: Ingresos
    public ActionResult Nuevo()
    {
        Libro nuevoLibro = new Libro();
       

        //le pasamos el modelo a la vista llamada NUEVO
        return View(nuevoLibro);
    }

    [HttpPost]
    public ActionResult AddNew(Libro nuevoLibro)
    {
        OMBContext ctx = DB.Contexto;

        try
        {
            ctx.Libros.Add(nuevoLibro);
            ctx.SaveChanges();
            return View("Resultado");
        }
        catch
        {
            return new HttpUnauthorizedResult();
        }
    }




  }
}