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
using WindowsOMB.Common;
using WindowsOMB.ViewModel;

namespace WindowsOMB.View
{
  /// <summary>
  /// Interaction logic for winLogin.xaml
  /// </summary>
  public partial class winLogin : Window
  {
    private readonly LoginViewModel _viewModel;
    private bool _perfilesActivo;

    public winLogin()
    {
      InitializeComponent();

      _perfilesActivo = false;
      _viewModel = new LoginViewModel(ProcessRequest);
      loginContainer.Content = new LoginCredenciales(_viewModel);
    }

    private void ProcessRequest(ActionRequest request)
    {
      switch (request)
      {
        case ActionRequest.CloseOK:
          if (_perfilesActivo)
            Close();
          else
          {
            _perfilesActivo = true;
            loginContainer.Content = new LoginPerfiles(_viewModel);
          }
          break;

        case ActionRequest.CloseCancel:
          Close();
          break;
      }
    }
  }
}
