using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOMB.Models;

namespace MvcOMB.Controllers
{
  public class ListadosController : Controller
  {
    public ActionResult List()
    {
      return View(new ListadosViewModel());
    }
  }
}