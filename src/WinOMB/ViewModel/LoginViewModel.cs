using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

using Servicios;
using Entidades;
using Entidades.Seguridad;

namespace WindowsOMB.ViewModel
{
  public class LoginViewModel: INotifyPropertyChanged
  {
    private CommandIngresar _cmdIngresar;
    private CommandCancelar _cmdCancelar;

    public event PropertyChangedEventHandler PropertyChanged;

    public event EventHandler LoginCancel;

    public LoginViewModel()
    {
      UsuarioModel = new Usuario();
      //  UsuarioModel.Login = "mburns";

      ComandoIngresar = new CommandIngresar(() =>
      {
        Debug.WriteLine(string.Format("Llamar a Login Usuario {0}", UsuarioModel.Login));
      });
      ComandoCancelar = new CommandCancelar(() =>
      {
        if (LoginCancel != null)
          LoginCancel(this, null);
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

    private Usuario _usuario;
    public Usuario UsuarioModel
    {
      get { return _usuario;  }
      set
      {
        _usuario = value;
        OnPropertyChanged("UsuarioModel");
      }
    }

    private void OnPropertyChanged(string prop)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(prop));
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
