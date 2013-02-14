using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ViewModels;
using System.Windows;

namespace Client.Controllers
{
    public class WindowLoader : IWindowLoader
    {

        private Dictionary<Type, Type> _ViewDictionary;


        public WindowLoader()
        {
            _ViewDictionary = new Dictionary<Type, Type>();

            _ViewDictionary.Add(typeof(LoginViewModel), typeof(LoginWindow));
            _ViewDictionary.Add(typeof(RegistrationViewModel), typeof(RegistrationWindow));
            _ViewDictionary.Add(typeof(MainWindowViewModel), typeof(MainWindow));
        }


        public void ShowView(IWindow viewModel)
        {
            Type viewModelType = viewModel.GetType();

            if (_ViewDictionary.ContainsKey(viewModelType))
            {
                Type viewType = _ViewDictionary[viewModelType];

                Window myView = (Window) Activator.CreateInstance(viewType);

                viewModel.WindowLoader = this;
                viewModel.RequestClose += delegate(object sender, EventArgs e) { myView.Close(); };

                myView.DataContext = viewModel;
                myView.Show();
            }
        }

    }
}
