using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Helpers;
using System.Windows.Input;
using Client.Factories;

namespace Client.ViewModels.Windows
{

    public interface IRegistrationSuccessPanelViewModel
    {
        String UserName { get; set; }
    }

    public class RegistrationSuccessPanelViewModel : ObservableObject, IRegistrationSuccessPanelViewModel
    {

        private String _UserName;

        private IWindowFactory _WindowFactory;
        private IRegistrationViewModel _RegistrationWindow;

        private ICommand _ShowMainWindowCommand;


        public RegistrationSuccessPanelViewModel(String username, IRegistrationViewModel regwindow, IWindowFactory winfactory)
        {
            _UserName = username;
            _RegistrationWindow = regwindow;

            _WindowFactory = winfactory;
        }


        #region Commands


        public ICommand ShowLoginWindowCommand
        {
            get
            {
                if (_ShowLoginWindowCommand == null)
                {
                    _ShowMainWindowCommand = new RelayCommand(param => this.ShowRegistrationWindow());
                }

                return _ShowMainWindowCommand;
            }
        }


        #endregion Commands


        public String UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnPropertyChanged("UserName"); }
        }


        private void ShowRegistrationWindow()
        {
            _WindowFactory.CreateLoginWindow().Show();
            _RegistrationWindow.RequestClose(_RegistrationWindow, null);
        }


        public object _ShowLoginWindowCommand { get; set; }
    }
}
