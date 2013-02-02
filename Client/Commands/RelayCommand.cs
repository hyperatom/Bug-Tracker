using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Diagnostics;

namespace Client.Commands
{
    /// <summary>
    /// This is a helper class which abstracts many of the commanding
    /// operations
    /// </summary>
    public class RelayCommand : ICommand
    {

        // The action to perform when command invoked
        readonly Action<object> _execute;
        // Predicate which determines if the command can invoke
        readonly Predicate<object> _canExecute;


        /// <summary>
        /// Constructor takes only an action if no CanExecute predicate defined.
        /// </summary>
        /// <param name="execute">The method delegated to the command.</param>
        public RelayCommand(Action<object> execute) : this(execute, null) { }


        /// <summary>
        /// Constructor takes both action and predicate. Predicate is evaluated
        /// to determine if the action is able to execute.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }


        /// <summary>
        /// Check if the delegated action can execute using this
        /// method.
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
