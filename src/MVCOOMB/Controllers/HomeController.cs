using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcOMB.Controllers
{
  public class HomeController : Controller
  {
    // GET: Home
    public ActionResult Inicio()
    {
        return View();  //  por default busca la vista Inicio.cshtml
    }
  }
}