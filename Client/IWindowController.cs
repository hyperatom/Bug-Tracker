using System;
namespace Client
{
    public interface IWindowController
    {
        void CloseLoginWindow();
        void CloseMainWindow();
        void CloseRegistrationWindow();
        void ShowRegistrationWindow();
        void ShowLoginWindow();
        void ShowMainWindow();
    }
}
