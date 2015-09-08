using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsOMB.Common
{
  /// <summary>
  /// Tipos de acciones que un VM puede estar avisando a una View
  /// </summary>
  public enum ActionRequest
  {
    /// <summary>
    /// Cierre de la vista sin errores
    /// </summary>
    CloseOK,   

    /// <summary>
    /// Cierre de la vista por cancelacion
    /// </summary>
    CloseCancel,

    /// <summary>
    /// 
    /// </summary>
    CloseError,

    /// <summary>
    /// 
    /// </summary>
    Close
  }
}
