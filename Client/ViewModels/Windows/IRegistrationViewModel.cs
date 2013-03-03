using System;
using System.Windows.Input;
using Client.Helpers;

namespace Client.ViewModels.Windows
{
    public interface IRegistrationViewModel : IWindow
    {
        ICommand CancelCommand { get; }
        bool CanRegister();
        string Email { get; set; }
        string Error { get; }
        string FirstAndLastName { get; set; }
        bool IsRegisterButtonEnabled { get; set; }
        string Organisation { get; set; }
        string Password { get; set; }
        ICommand RegisterCommand { get; }
        void RegistrationComplete(IAsyncResult result);
        string this[string Field] { get; }
    }
}
