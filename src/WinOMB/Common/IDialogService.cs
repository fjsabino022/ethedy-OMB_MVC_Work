using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WindowsOMB.Common
{
  /// <summary>
  /// Interface que puede implementarse en una vista concreta para ser invocada desde el view model
  /// </summary>
  public interface IDialogService
  {
    /// <summary>
    /// Un metodo que invoca una interface visual
    /// </summary>
    void Show();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="notificacion"></param>
    void Show<T>(Action<T> notificacion);
  }
}
