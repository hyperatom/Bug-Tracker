using System;
using Client.Helpers;
using System.Windows.Input;

namespace Client.ViewModels.Windows
{
    public interface ILoginViewModel : IWindow
    {
        bool IsRememberMeChecked { get; set; }
        void Login();
        ICommand LoginCommand { get; }
        string Password { get; set; }
        ICommand RegistrationCommand { get; }
        string Username { get; set; }
    }
}
