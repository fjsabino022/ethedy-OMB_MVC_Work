using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsOMB.Common;
using WindowsOMB.View;
using Infraestructura;

namespace WindowsOMB.ViewModel
{
  /// <summary>
  /// Contiene logica asociada a la ventana principal
  /// Binding de botones de la ribbon
  /// Binding de status bar
  /// </summary>
  public class MainViewModel : ViewModelBase
  {
    public ComandoSimple LoginCommand { get; set; }
    public ComandoSimple LogoutCommand { get; set; }

    public MainViewModel()
    {
      LoginCommand = new ComandoSimple(TryLogin, () => Context.Current.Sesion == null);
      LogoutCommand = new ComandoSimple(Logout, () => Context.Current.Sesion != null);
    }

    private void Logout()
    {
      //
    }

    internal void TryLogin()
    {
      LoginService login = Context.Current.ServiceProvider.GetService(typeof(LoginService)) as LoginService;

      if (login != null)
      {
        login.Show();

        if (Context.Current.Sesion != null)
        {
          //  actualizar comandos
          //  mostrar usuario conectado
          //  permitir activar nuevas opciones de ribbon
        }
      }
    }
  }
}
