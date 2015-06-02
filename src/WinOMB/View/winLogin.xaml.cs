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
using WindowsOMB.ViewModel;

namespace WindowsOMB.View
{
  /// <summary>
  /// Interaction logic for winLogin.xaml
  /// </summary>
  public partial class winLogin : Window
  {
    private LoginViewModel _viewModel;

    public winLogin()
    {
      InitializeComponent();

      _viewModel = new LoginViewModel();
      loginContainer.Content = new LoginCredenciales(_viewModel);
    }
  }
}
