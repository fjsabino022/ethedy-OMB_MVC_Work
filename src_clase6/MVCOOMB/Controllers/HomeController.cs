using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidades;
using Servicios;

namespace MvcOMB.Controllers
{
  public class HomeController : Controller
  {
    // GET: Home
    public ActionResult Inicio()
    {
        return View();  //  por default busca la vista Inicio.cshtml
    }

    [HttpGet]
    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Login(string txtLogin, string txtPassword)
    {
      SecurityServices serv = new SecurityServices();
      Usuario user;
      ActionResult result = new EmptyResult();

      try
      {
        user = serv.Login(txtLogin, txtPassword);
        if (user != null)
        {
          //  Opcion 1: terminamos aca y no pedimos perfil
          //  result = View("LoginOK", user);

          //  Opcion 2: redirigimos a otra vista que nos permite elegir el perfil (salvo que tenga un unico perfil...)
          if (user.Perfiles.Count > 1)
          {
            result = View("PerfilSelect", user);
          }
          else
          {
            //  Guardamos los datos de sesion en el "contexto" de la sesion establecida (similar al Context que usamos en WPF)
            //
            Session["SESION_USER"] = serv.CrearSesion(user, user.Perfiles.Single());

            //  creamos una nueva vista strong-typed para incorporar la Sesion
            result = View("LoginOK_v2", Session["SESION_USER"] as Sesion);
          }
        }
        else
        {
          //  TODO: armar paginas de error para los casos de credenciales incorrectas o excepcion
          //  TODO: y un controlador que ademas realice el log del problema?
          //
          result = new HttpUnauthorizedResult("Credenciales incorrectas");
        }
      }
      catch (Exception ex)
      {
        //  redireccionar a una pagina de error!!
        result = new HttpUnauthorizedResult("Estas al horno!!!");
      }
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cbPerfiles">Valor option del elemento actualmente seleccionado en el dropdown</param>
    /// <param name="login">Argumento adicional que se obtiene desde el modelo y se vuelve a pasar al controlador
    /// Otra posibilidad seria incluirlo en un campo hidden
    /// </param>
    /// <returns></returns>
    public ActionResult SelectPerfil(string cbPerfiles, string login)
    {
      SecurityServices serv = new SecurityServices();
      Usuario user = serv.GetUsuarioFromLogin(login);
      Perfil perfilElegido;
      Sesion newSesion;

      perfilElegido = user.Perfiles.Where(pf => pf.Nombre == cbPerfiles).Single();

      Session["SESION_USER"] = newSesion = serv.CrearSesion(user, perfilElegido);
      return View("LoginOK_v2", newSesion);
    }

    public ActionResult Logout()
    {
      Sesion sesionActual = Session["SESION_USER"] as Sesion;

      sesionActual.Logout();
      Session.Remove("SESION_USER");
      return View("Inicio");
    }
  }
}