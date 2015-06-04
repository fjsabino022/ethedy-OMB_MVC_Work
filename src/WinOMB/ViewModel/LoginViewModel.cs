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

using Servicios;
using Entidades;

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
          OnLoginOK();
        }
        else
          OnLoginCancel();
      });

      ComandoIngresarPerfil = new CommandIngresar(() =>
      {
        Debug.WriteLine(_perfil.Descripcion);
        OnLoginOK();
      });

      ComandoCancelar = new CommandCancelar(() =>
      {
        OnLoginCancel();
      });
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
