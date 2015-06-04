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

      LoginCommand.OnCanExecuteChanged();
      LogoutCommand.OnCanExecuteChanged();
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

  public class ComandoSimple : ICommand
  {
    private Action<object> _execute;
    private Func<object, bool> _canExecute;

    public ComandoSimple(Action<object> exec, Func<object, bool> canExec)
    {
      _execute = exec;
      _canExecute = canExec;
    }

    public ComandoSimple(Action<object> exec)
    {
      _execute = exec;
      _canExecute = (o) => true;
    }

    public bool CanExecute(object parameter)
    {
      return _canExecute(parameter);
    }

    public event EventHandler CanExecuteChanged;

    public void OnCanExecuteChanged()
    {
      if (CanExecuteChanged != null)
        CanExecuteChanged(this, new EventArgs());
    }

    public void Execute(object parameter)
    {
      _execute(parameter);
    }
  }
}
