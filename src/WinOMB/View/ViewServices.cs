using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsOMB.View
{
  public class LoginService
  {
    public void Show()
    {
      winLogin login = new winLogin();
      login.ShowDialog();
    }
  }
}
