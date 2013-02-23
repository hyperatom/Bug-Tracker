using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using System.Windows;
using Client.ViewModels;

namespace Client.Controllers
{
    /// <summary>
    /// This class allows the loading of views bound to their
    /// corresponding view model classes.
    /// </summary>
    public class WindowLoader : IWindowLoader
    {
        // Stores the type mappings between view-viewmodel
        private Dictionary<Type, Type> _ViewDictionary;
        private IMessenger _Messenger;


        /// <summary>
        /// Adds type mappings between view-viewmodels to dictionary.
        /// </summary>
        public WindowLoader(IMessenger comm)
        {
            _ViewDictionary = new Dictionary<Type, Type>();
            _Messenger = comm;

            _ViewDictionary.Add(typeof(LoginViewModel), typeof(LoginWindow));
            _ViewDictionary.Add(typeof(RegistrationViewModel), typeof(RegistrationWindow));
            _ViewDictionary.Add(typeof(MainWindowViewModel), typeof(MainWindow));
        }


        /// <summary>
        /// Takes a view model as argument and displays the
        /// corresponding view bound to the inputted view model.
        /// 
        /// If view model to view mapping is not in dictionary,
        /// throw an invalid argument execption.
        /// </summary>
        /// <param name="viewModel">The view model which binds to the view to be shown.</param>
        public void ShowView(IWindow viewModel)
        {
            Type viewModelType = viewModel.GetType();

            // If mapping is found, create a new instance of the view
            if (_ViewDictionary.ContainsKey(viewModelType))
            {
                Type viewType = _ViewDictionary[viewModelType];

                // Create new window of corresponding type found in dictionary
                Window myView = (Window)Activator.CreateInstance(viewType);

                // Provide view model with reference to this class
                viewModel.WindowLoader = this;
                // Allow the view model to close its corresponding view
                viewModel.RequestClose += delegate(object sender, EventArgs e) { myView.Close(); };

                // Bind the view model to the view
                myView.DataContext = viewModel;
                myView.Show();
            }
            else
            {
                // Throw exception if no mapping between view model and view are found
                throw new ArgumentException("This window is not supported by the window loader.");
            }
        }

    }
}
