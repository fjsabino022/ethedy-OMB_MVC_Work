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
using Infraestructura;

namespace WindowsOMB.View
{
  /// <summary>
  /// Interaction logic for winTest.xaml
  /// </summary>
  public partial class winTest : Window
  {
    private TestViewModel _viewModel;

    public winTest()
    {
      InitializeComponent();

      _viewModel = new TestViewModel();
      this.DataContext = _viewModel;

      Context.Current.ServiceProvider.AddService(typeof(LoginService), new LoginService());
    }

    private void TestLoaded(object sender, RoutedEventArgs e)
    {
      _viewModel.TryLogin();
      if (Context.Current.Sesion != null)
      {
        txtUsuario.Text = string.Format("Usuario conectado: {0}", Context.Current.Sesion.FullName);
        txtPerfil.Text = string.Format("Perfil Activo: {0}", Context.Current.Sesion.Perfil.Descripcion);
      }
    }
  }
}
