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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsOMB.ViewModel;

namespace WindowsOMB.View
{
  /// <summary>
  /// Interaction logic for LoginPerfiles.xaml
  /// </summary>
  public partial class LoginPerfiles : UserControl
  {
    private LoginViewModel _vm;

    public LoginPerfiles(LoginViewModel viewModel)
    {
      InitializeComponent();
      _vm = viewModel;
    }

    private void ControlLoaded(object sender, RoutedEventArgs e)
    {
      this.DataContext = _vm;
    }


  }
}
