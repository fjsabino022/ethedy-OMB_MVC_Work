using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsOMB.Common;

namespace WindowsOMB.ViewModel
{
  public class ExceptionNotificationViewModel : ViewModelBase
  {
    private Action<ActionRequest> _notify;
    private string _mensaje, _titulo;

    public ExceptionNotificationViewModel(Action<ActionRequest> notify)
    {
      _notify = notify ?? delegate(ActionRequest r) { };
      ComandoAceptar = new ComandoSimple(() => _notify(ActionRequest.Close));
    }

    public ComandoSimple ComandoAceptar { get; set; }

    public string Mensaje
    {
      get
      {
        return _mensaje;
      }
      set
      {
        _mensaje = value;
        OnPropertyChanged();
      }
    }

    public string Titulo
    {
      get
      {
        return _titulo;
      }
      set
      {
        _titulo = value;
        OnPropertyChanged();
      }
    }
  }
}
