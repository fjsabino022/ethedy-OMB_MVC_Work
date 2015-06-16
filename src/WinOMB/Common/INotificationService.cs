using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsOMB.Common
{
  /// <summary>
  /// 
  /// </summary>
  public interface INotificationService
  {
    string Mensaje { get; set; }
    string Titulo { get; set; }

    /// <summary>
    /// Un metodo que muestra la notificacion y no retorna ningun feedback
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
