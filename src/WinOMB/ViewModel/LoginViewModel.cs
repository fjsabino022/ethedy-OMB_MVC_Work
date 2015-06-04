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

namespace WindowsOMB.ViewModel
{
  public class LoginViewModel: INotifyPropertyChanged
  {
    private CommandIngresar _cmdIngresar;
    private CommandIngresar _cmdIngresarPerfil;
    private CommandCancelar _cmdCancelar;
    private Usuario _usuario;
    private string _password;
    private Perfil _perfil;
    private ObservableCollection<Perfil> _perfiles;

    public event PropertyChangedEventHandler PropertyChanged;

    public event EventHandler LoginCancel;

    public event EventHandler LoginOK;

    public LoginViewModel()
    {
      UsuarioModel = new Usuario();
      //  UsuarioModel.Login = "mburns";

      ComandoIngresar = new CommandIngresar(() =>
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

      ComandoIngresarPerfil = new CommandIngresar(() =>
      {
        SecurityServices srv = new SecurityServices();

        Context.Current.Sesion = srv.CrearSesion(UsuarioModel, PerfilSeleccionado);

        Debug.WriteLine(_perfil.Descripcion);

        OnLoginOK();
      });

      ComandoCancelar = new CommandCancelar(() =>
      {
        OnLoginCancel();
      });

      PerfilSeleccionado = null;
    }

    public CommandIngresar ComandoIngresar
    {
      get { return _cmdIngresar; }
      set
      {
        _cmdIngresar = value;
        OnPropertyChanged("ComandoIngresar");
      }
    }

    public CommandCancelar ComandoCancelar
    {
      get { return _cmdCancelar; }
      set
      {
        _cmdCancelar = value;
        OnPropertyChanged("ComandoCancelar");
      }
    }

    public CommandIngresar ComandoIngresarPerfil
    {
      get { return _cmdIngresarPerfil; }
      set
      {
        _cmdIngresarPerfil = value;
        OnPropertyChanged("ComandoIngresarPerfil");
      }
    }

    public Usuario UsuarioModel
    {
      get { return _usuario;  }
      set
      {
        _usuario = value;
        OnPropertyChanged("UsuarioModel");
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
        OnPropertyChanged("Password");
      }
    }

    public Perfil PerfilSeleccionado
    {
      get { return _perfil; }
      set
      {
        _perfil = value;
        OnPropertyChanged("PerfilSeleccionado");
      }
    }

    public ObservableCollection<Perfil> Perfiles
    {
      get { return _perfiles; }
      set
      {
        _perfiles = value;
        OnPropertyChanged("Perfiles");
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

    private void OnPropertyChanged(string prop)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }

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
  }

  public class CommandIngresar : ICommand
  {
    private Action _comando;

    public CommandIngresar(Action comandoAction)
    {
      _comando = comandoAction;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      _comando();
    }

    public event EventHandler CanExecuteChanged;
  }

  public class CommandCancelar : ICommand
  {
    private Action _comando;

    public CommandCancelar(Action comandoAction)
    {
      _comando = comandoAction;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      _comando();
    }

    public event EventHandler CanExecuteChanged;
  }
}
