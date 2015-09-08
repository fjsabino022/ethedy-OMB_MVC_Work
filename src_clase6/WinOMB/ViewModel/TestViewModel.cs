using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using Infraestructura;
using WindowsOMB.View;
using System.Windows.Input;
using WindowsOMB.Common;

namespace WindowsOMB.ViewModel
{
  public class TestViewModel : ViewModelBase
  {
    private ComandoSimple _cmdLogin;
    private ComandoSimple _cmdLogout;

    public TestViewModel()
    {
      LoginCommand = new ComandoSimple((x) =>
      {
        TryLogin();
      },
      (x) => 
      {
        return Context.Current.Sesion == null;
        //  return true;
      });

      LogoutCommand = new ComandoSimple(
        (x) => 
        {
          //
        },
        (x) =>
        {
          return Context.Current.Sesion != null;
        });
    }

    public void TryLogin()
    {
      var login = Context.Current.ServiceProvider.GetService(typeof(LoginService)) as LoginService;

      login.Show();
    }

    public ComandoSimple LoginCommand
    { 
      get { return _cmdLogin; }
      set
      {
        _cmdLogin = value;
        OnPropertyChanged("LoginCommand");
      }
    }

    public ComandoSimple LogoutCommand
    {
      get { return _cmdLogout; }
      set
      {
        _cmdLogout = value;
        OnPropertyChanged("LogoutCommand");
      }
    }
  }
}
