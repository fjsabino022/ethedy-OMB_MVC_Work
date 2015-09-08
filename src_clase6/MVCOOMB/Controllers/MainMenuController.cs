using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOMB.Models;
using Servicios;

namespace MvcOMB.Controllers
{
  public class MainMenuController : Controller
  {
    // GET: MainMenu
    public PartialViewResult Menu(string menuActual = null)
    {
      MainMenuViewModel vm = new MainMenuViewModel(Session["SESION_USER"] as Sesion);
      if (menuActual != null)
        ViewBag.MenuActual = menuActual;

      return PartialView(vm);
    }
  }
}