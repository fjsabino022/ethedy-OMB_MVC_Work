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
using Database;
using WindowsOMB.Common;

namespace WindowsOMB.ViewModel
{
  public class LoginViewModel: ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
  {
    private ComandoSimple _cmdIngresar;
    private ComandoSimple _cmdIngresarPerfil;
    private ComandoSimple  _cmdCancelar;
    private Usuario _usuario;
    private string _password;
    private Perfil _perfil;
    private ObservableCollection<Perfil> _perfiles;

    private string _login;

    public event EventHandler LoginCancel;

    public event EventHandler LoginOK;

    public LoginViewModel()
    {
      UsuarioModel = new Usuario();
      //  UsuarioModel.Login = "mburns";

      ComandoIngresar = new ComandoSimple(() =>
      {
        Debug.WriteLine(string.Format("Llamar a Login Usuario {0} , Pass: {1}", UsuarioModel.Login, Password));

        SecurityServices srv = new SecurityServices();

        if ((UsuarioModel = srv.Login(UsuarioModel, Password)) != null)
        {
          Perfiles = new ObservableCollection<Perfil>(UsuarioModel.Perfiles);
          PerfilSeleccionado = Perfiles[0];

          OnLoginOK();
        }
        else
          OnLoginCancel();
      });

      ComandoIngresarPerfil = new ComandoSimple(() =>
      {
        SecurityServices srv = new SecurityServices();

        Context.Current.Sesion = srv.CrearSesion(UsuarioModel, PerfilSeleccionado);

        Debug.WriteLine(_perfil.Descripcion);

        OnLoginOK();
      });

      ComandoCancelar = new ComandoSimple(() =>
      {
        OnLoginCancel();
      });

      PerfilSeleccionado = null;
    }

    public string Login
    {
      get { return _login; }
      set
      {
        if (string.IsNullOrEmpty(value) || value.Length < 5)
          throw new ArgumentException("Valor de login inadecuado");

        _login = value;
        OnPropertyChanged();
      }
    }

    public ComandoSimple ComandoIngresar
    {
      get { return _cmdIngresar; }
      set
      {
        _cmdIngresar = value;
        OnPropertyChanged();
      }
    }

    public ComandoSimple ComandoCancelar
    {
      get { return _cmdCancelar; }
      set
      {
        _cmdCancelar = value;
        OnPropertyChanged();
      }
    }

    public ComandoSimple ComandoIngresarPerfil
    {
      get { return _cmdIngresarPerfil; }
      set
      {
        _cmdIngresarPerfil = value;
        OnPropertyChanged();
      }
    }

    public Usuario UsuarioModel
    {
      get { return _usuario;  }
      set
      {
        _usuario = value;
        OnPropertyChanged();
      }
    }

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

    public void Sorprender()
    {
      Task tsk = new Task(() =>
      {
        Thread.Sleep(3000);
        //  cambiamos el nombre del usuario 
        _usuario.Login += _usuario.Login;
      });
      tsk.Start();
    }

    #region LANZADORES DE EVENTOS

    private void OnLoginOK()
    {
      if (LoginOK != null)
        LoginOK(this, new EventArgs());
    }

    private void OnLoginCancel()
    {
      if (LoginCancel != null)
        LoginCancel(this, new EventArgs());
    }

    private void OnLoginError(string errMsg)
    {
      //  TODO enviar evento de error 
    }

    #endregion

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
            if (string.IsNullOrWhiteSpace(_password) || _password.Length < 10)
              return "La password debe ser de al menos 10 caracteres";
            else
              return string.Empty;
            break;
        }
        return string.Empty;
      }
    }
  }
}
