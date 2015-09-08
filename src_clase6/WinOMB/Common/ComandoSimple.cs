using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowsOMB.Common
{
  /// <summary>
  /// Representa un comando simple con metodos Execute y CanExecute para utilizar como activadores de tareas relacionadas
  /// a botones y otros controles WPF
  /// </summary>
  public class ComandoSimple : ICommand
  {
    private Action<object> _Execute { get; set; }
    private Func<object, bool> _CanExecute { get; set; }

    /// <summary>
    /// Permite declarar un comando sin parametros que siempre puede ser ejecutado
    /// Util si la accion no necesita hacer uso del object asociado a Execute
    /// Ademas CanExecute retorna siempre true
    /// </summary>
    /// <param name="execute"></param>
    public ComandoSimple(Action execute)
      : this(o => execute(), o => true)
    {
      //  comando sin parametros que se ejecuta siempre
    }

    /// <summary>
    /// Permite declarar un comando con un parametro que siempre puede ser ejecutado
    /// Util si la accion necesita hacer uso del object asociado a Execute pasado desde el binding de WPF por ejemplo
    /// CanExecute retorna siempre true
    /// </summary>
    /// <param name="execute"></param>
    public ComandoSimple(Action<object> execute)
      : this(o => execute(o), o => true)
    {
      //  comando con un parametro que se ejecuta siempre
    }

    /// <summary>
    /// Permite declarar un comando casi como ICommand pero que no utiliza el parametro adicional en ninguno de los metodos
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public ComandoSimple(Action execute, Func<bool> canExecute)
      : this(o => execute(), o => canExecute())
    {
      //
    }

    /// <summary>
    /// Permite declarar un comando completo como lo especifica la interface ICommand
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public ComandoSimple(Action<object> execute, Func<object, bool> canExecute)
    {
      _Execute = execute;
      _CanExecute = canExecute;
      //
    }

    public bool CanExecute(object parameter)
    {
      return _CanExecute(parameter);
    }

    public void Execute(object parameter)
    {
      _Execute(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }
  }
}
