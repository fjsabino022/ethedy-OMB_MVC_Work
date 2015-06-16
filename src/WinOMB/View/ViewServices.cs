using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsOMB.Common;
using WindowsOMB.ViewModel;

namespace WindowsOMB.View
{
  public class LoginService : IDialogService
  {
    public void Show()
    {
      winLogin login = new winLogin();
      login.ShowDialog();
    }

    public void Show<T>(Action<T> notificacion)
    {
      throw new NotImplementedException();
    }
  }

  public class ExceptionNotificacionService : INotificationService
  {
    private ExceptionNotificationView _view;
    private ExceptionNotificationViewModel _vm;
    private winNotification _winNotification;

    public string Mensaje { get { return _vm.Mensaje;  } set { _vm.Mensaje = value; } }
    public string Titulo { get { return _vm.Titulo; } set { _vm.Titulo = value; } }

    public ExceptionNotificacionService()
    {
      _vm = new ExceptionNotificationViewModel(request => { 
        if (_winNotification != null) 
          _winNotification.Close();
      } );
      _view = new ExceptionNotificationView(_vm);
    }

    public void Show()
    {
      _winNotification = new winNotification(_view);

      _winNotification.ShowDialog();
    }

    public void Show<T>(Action<T> notificacion)
    {
      throw new NotImplementedException();
    }
  }
}
