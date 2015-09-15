using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
using WindowsOMB.Common;
using WindowsOMB.ViewModel;
using Infraestructura;
using Syncfusion.Windows.Tools.Controls;

namespace WindowsOMB.View
{
  /// <summary>
  /// Interaction logic for winMain.xaml
  /// </summary>
  public partial class winMain : RibbonWindow
  {
    private MainViewModel _viewModel;

    public winMain()
    {
      InitializeComponent();

      _viewModel = new MainViewModel();
      this.DataContext = _viewModel;

      Context.Current.ServiceProvider.AddService(typeof(IDialogService), new LoginService());
      Context.Current.ServiceProvider.AddService(typeof(INotificationService), new ExceptionNotificacionService());
    }

    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
      //  ribMain.BackStageButton.Visibility = Visibility.Collapsed;
      _viewModel.TryLogin();
    }
  }
}
