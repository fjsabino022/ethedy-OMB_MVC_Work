using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WindowsOMB.View
{
  /// <summary>
  /// Una ventana de notificacion general que sirve para cualquier proposito, dependiendo del control que coloque en el contenedor
  /// </summary>
  public partial class winNotification : Window
  {
    public winNotification(Control ctrl)
    {
      InitializeComponent();

      notifyContent.Content = ctrl;
    }
  }
}
