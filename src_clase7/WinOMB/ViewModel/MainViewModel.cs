using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsOMB.Common;
using WindowsOMB.View;
using Infraestructura;
using Servicios;

namespace WindowsOMB.ViewModel
{
  /// <summary>
  /// Contiene logica asociada a la ventana principal
  /// Binding de botones de la ribbon
  /// Binding de status bar
  /// </summary>
  public class MainViewModel : ViewModelBase
  {
    private bool _isUserConnected;
    private string _userConnectedName;

    public ComandoSimple LoginCommand { get; set; }
    public ComandoSimple LogoutCommand { get; set; }

    public MainViewModel()
    {
      UserConnectedName = "<Desconectado>";
      UserConnected = false;

      LoginCommand = new ComandoSimple(TryLogin, () => Context.Current.Sesion == null);
      LogoutCommand = new ComandoSimple(Logout, () => Context.Current.Sesion != null);
    }

    public bool UserConnected
    {
      get { return _isUserConnected; }
      set
      {
        _isUserConnected = value;
        OnPropertyChanged();
      }
    }

    public string UserConnectedName
    {
      get { return _userConnectedName; }
      set
      {
        _userConnectedName = value;
        OnPropertyChanged();
      }
    }

    private void Logout()
    {
      SecurityServices serv = new SecurityServices();

      serv.CerrarSesion();

      UserConnectedName = "<Desconectado>";
      UserConnected = false;
    }

    internal void TryLogin()
    {
      IDialogService login = Context.Current.ServiceProvider.GetService(typeof(IDialogService)) as IDialogService;

      if (login != null)
      {
        login.Show();

        if (Context.Current.Sesion != null)
        {
          UserConnected = true;
          UserConnectedName = Context.Current.Sesion.FullName;
        }
      }
    }
  }
}
