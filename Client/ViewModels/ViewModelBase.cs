using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using Client.ServiceReference;

namespace Client.ViewModels
{
    /// <summary>
    /// This is an abstract, generic class which takes a view as a parameter.
    /// The generic type allows any view model access to its corresponding view.
    /// Each view model is also passed a reference to the window controller which
    /// gives it access to every other view model.
    /// </summary>
    /// <typeparam name="T">A view.</typeparam>
    public abstract class ViewModelBase<T> where T : Window, new() 
    {

        // Reference to the view
        protected T _View;
        protected TrackerServiceClient _Service;
        protected IWindowController _Controller;


        public ViewModelBase() { }


        /// <summary>
        /// Copies reference of window controller to global variable.
        /// </summary>
        /// <param name="controller">Reference to window controller.</param>
        public ViewModelBase(IWindowController controller, TrackerServiceClient service)
        {
            _Controller = controller;
            _Service = service;
        }


        /// <summary>
        /// Any view model can invoke this method to display its
        /// corresponding view and initialise the datacontext to
        /// the instance of the object.
        /// </summary>
        public void ShowView()
        {
            _View = new T();
            _View.Show();
            _View.DataContext = this;
        }


        /// <summary>
        /// Any view model can access this method in order to close
        /// the corresponding view.
        /// </summary>
        public void CloseView()
        {
            if (_View != null) _View.Close();
        }
    
    }
}
