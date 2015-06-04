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
  /// Interaction logic for LoginCredenciales.xaml
  /// </summary>
  public partial class LoginCredenciales : UserControl
  {
    private LoginViewModel _viewModel;

    public LoginCredenciales(LoginViewModel vm)
    {
      InitializeComponent();
      _viewModel = vm;
    }

    private void ControlLoaded(object sender, RoutedEventArgs e)
    {
      this.DataContext = _viewModel;

      //  btnIngresar.CommandBindings.Add(new CommandBinding(_viewModel.ComandoIngresar));
      txtUsuario.Focus();
    }

    private void PasswordFocus(object sender, RoutedEventArgs e)
    {
      //  _viewModel.Sorprender();
    }
  }
}
