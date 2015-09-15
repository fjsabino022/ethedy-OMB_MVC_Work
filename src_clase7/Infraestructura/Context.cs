using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Design;
using Servicios;

namespace Infraestructura
{
  /// <summary>
  /// SINGLETON
  /// Esta clase contiene propiedades que serán comunes para toda la aplicación, no importa el usuario que este autenticado
  /// o bien que no haya ningun usuario autenticado
  /// </summary>
  public class Context
  {
    private static readonly Context _context;

    /// <summary>
    /// Retorna una referencia al Contexto actual y UNICO de la aplicacion
    /// </summary>
    public static Context Current { get { return _context; } }

    static Context()
    {
      _context = new Context();
    }

    private Context()
    {
      Sesion = null;
      ServiceProvider = new ServiceContainer();
    }

    //  Propiedades publicas DE LA INSTANCIA
    public Sesion Sesion { get; set; }

    public IServiceContainer ServiceProvider { get; set; }
  }
}
