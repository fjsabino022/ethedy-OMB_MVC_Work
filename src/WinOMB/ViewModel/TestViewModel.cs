using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using Infraestructura;
using WindowsOMB.View;

namespace WindowsOMB.ViewModel
{
  public class TestViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public TestViewModel()
    {
      
    }

    public void TryLogin()
    {
      var login = Context.Current.ServiceProvider.GetService(typeof(LoginService)) as LoginService;

      login.Show();
    }
  }
}
