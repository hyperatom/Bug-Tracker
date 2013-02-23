using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Diagnostics;

namespace Client.Helpers
{
    /// <summary>
    /// This class abstracts commanding operations using the
    /// ICommand interface. Taken from the Microsoft web page:
    /// http://msdn.microsoft.com/en-us/library/microsoft.teamfoundation.mvvm.relaycommand.aspx
    /// </summary>
    public class RelayCommand : ICommand
    {

        // The action to perform when command is executed
        readonly Action<object> _execute;
        // Predicate which determines if the command can be executed
        readonly Predicate<object> _canExecute;


        /// <summary>
        /// Constructor takes one parameter as the action to be executed
        /// and assumes _canExecute is always true.
        /// </summary>
        /// <param name="execute">The method delegated to execute.</param>
        public RelayCommand(Action<object> execute) : this(execute, null) { }


        /// <summary>
        /// Constructor takes both an action and predicate. Predicate is evaluated
        /// to determine if the action is able to execute.
        /// </summary>
        /// <param name="execute">The delegated method to execute.</param>
        /// <param name="canExecute">Predicate which determines if action can execute.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }


        /// <summary>
        /// Checks if the delegated command can execute using boolean
        /// member value.
        /// </summary>
        /// <param name="parameter">Optional custom parameter.</param>
        /// <returns>Return true if predicate is null, false otherwise.</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }


        /// <summary>
        /// Executes the delegated method using optional parameters.
        /// </summary>
        /// <param name="parameter">Optional custom parameter.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }


        /// <summary>
        /// Event which detects when the CanExecute property has changed.
        /// Re-evaluates the predicate when fired.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

    }
}
