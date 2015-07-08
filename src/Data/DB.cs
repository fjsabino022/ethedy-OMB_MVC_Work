using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
  /// <summary>
  /// [[SINGLETON]]
  /// Representa un punto de ingreso a la DB mediante el objeto DbContext de E-F
  /// </summary>
  public class DB
  {
    private static OMBContext _ctx;

    public static OMBContext Contexto
    {
      get
      {
        if (_ctx == null)
        {
          _ctx = new OMBContext();
          _ctx.VerboseMode = true;
        }
        return _ctx;
      }
    }

    private DB() { }
  }
}
