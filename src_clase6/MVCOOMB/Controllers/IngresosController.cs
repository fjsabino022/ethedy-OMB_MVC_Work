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

        //VALIDACIONES, ESTA BUENISIMO COMO SE HACEN
        //manejamos las validaciones de cada campo
        if (String.IsNullOrEmpty(nuevoLibro.ISBN13))
        {
            ModelState.AddModelError("ISBN13", "El campo debe estar completo");
        }
        if (String.IsNullOrEmpty(nuevoLibro.ISBN10))
        {
            ModelState.AddModelError("ISBN10", "El campo debe estar completo");
        }
        if (String.IsNullOrEmpty(nuevoLibro.Titulo))
        {
            ModelState.AddModelError("Titulo", "El campo debe estar completo");
        }
        else
        {    
            //cuando no le pones el atributo de la PROPIEDAd, se consideran que son errores del MODELO
            if (nuevoLibro.Titulo.Contains("xxx"))
            {
                ModelState.AddModelError("", "Esta Librerìa es porno");
            }
        }


        //SI EL MODELO ES VALIDO
        if (ModelState.IsValid)
        {
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
        else
        {
            return View("Nuevo", nuevoLibro);
        }
    }
  }
}