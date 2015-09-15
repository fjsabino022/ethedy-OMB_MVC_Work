using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

using System.Threading;

using Infraestructura;
using Servicios;
using Entidades;
using WindowsOMB.Common;

namespace WindowsOMB.ViewModel
{
  public class LoginViewModel: ViewModelBase, IDataErrorInfo
  {
    private string _password;
    private string _login;
    private Perfil _perfil;
    private ObservableCollection<Perfil> _perfiles;

    private Action<ActionRequest> _notify;

    public LoginViewModel(Action<ActionRequest> notify)
    {
      _notify = notify ?? delegate(ActionRequest r) { };

      ComandoIngresar = new ComandoSimple(() =>
      {
        Debug.WriteLine("Llamar a Login Usuario {0} , Pass: {1}", Login, Password);

        SecurityServices srv = new SecurityServices();

        try
        {
          if ((Usuario = srv.Login(Login, Password)) != null)
          {
            Perfiles = new ObservableCollection<Perfil>(Usuario.Perfiles);
            PerfilSeleccionado = Perfiles[0];

            _notify(ActionRequest.CloseOK);
          }
        }
        catch (OMBSecurityException ex)
        {
          OnLoginError(ex.Message);
        }
      }, IsValid);

      ComandoIngresarPerfil = new ComandoSimple(() =>
      {
        SecurityServices srv = new SecurityServices();

        srv.CrearSesion(Usuario, PerfilSeleccionado);

        Debug.WriteLine("Perfil Seleccionado: {0}", (object)_perfil.Descripcion);

        _notify(ActionRequest.CloseOK);
      });

      ComandoCancelar = new ComandoSimple(() => _notify(ActionRequest.CloseCancel));

      PerfilSeleccionado = null;
    }

    #region PROPIEDADES BINDEABLES

    /// <summary>
    /// Representa el ID de ingreso del usuario al sistema
    /// Validacion por excepcion...
    /// </summary>
    public string Login
    {
      get { return _login; }
      set
      {
        if (string.IsNullOrEmpty(value) || value.Length < 5)
          throw new ArgumentException("El Login debe ser un valor no vacio y de al menos 5 caracteres");

        _login = value;
        OnPropertyChanged();
      }
    }

    /// <summary>
    /// Contraseña que se tiene que usar para ingresar al sistema
    /// </summary>
    public string Password
    {
      private get
      {
        return _password;
      }
      set
      {
        _password = value;
        OnPropertyChanged();
      }
    }

    public Perfil PerfilSeleccionado
    {
      get { return _perfil; }
      set
      {
        _perfil = value;
        OnPropertyChanged();
      }
    }

    public ObservableCollection<Perfil> Perfiles
    {
      get { return _perfiles; }
      set
      {
        _perfiles = value;
        OnPropertyChanged();
      }
    }

    #endregion

    #region COMANDOS

    public ComandoSimple ComandoIngresar { get; set; }

    public ComandoSimple ComandoCancelar { get; set; }

    public ComandoSimple ComandoIngresarPerfil { get; set; }

    #endregion

    private Usuario Usuario { get; set; }

    public bool IsValid()
    {
      //  aqui el problema de tener que recorrer la coleccion de todas las propiedades que puedan producir error
      //  como en este caso es una sola, no es una complicacion
      foreach (var item in  new[] {"Password"})
        if (!string.IsNullOrEmpty(this[item]))
          return false;
      return true;
    }

    public void Sorprender()
    {
      Task tsk = new Task(() =>
      {
        Thread.Sleep(3000);
        //  cambiamos el nombre del usuario 
        Login += Login;
      });
      tsk.Start();
    }

    #region LANZADORES DE EVENTOS

    private void OnLoginError(string errMsg)
    {
      INotificationService serv = Context.Current.ServiceProvider.GetService(typeof (INotificationService)) as INotificationService;

      serv.Mensaje = errMsg;
      serv.Titulo = "ERROR IMPORTANTE";
      serv.Show();
    }

    #endregion

    #region Implementacion IDataErrorInfo

    public string Error
    {
      get { return string.Empty; }
    }

    public string this[string propName]
    {
      get
      {
        switch (propName)
        {
          case "Password":
            if (string.IsNullOrWhiteSpace(_password) || _password.Length < 5)
              return "La password no puede estar vacia y debe ser de al menos 5 caracteres";
            else
              return string.Empty;
            break;
        }
        return string.Empty;
      }
    }
    
    #endregion

  }
}
