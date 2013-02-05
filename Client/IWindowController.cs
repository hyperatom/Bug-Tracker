using System;
namespace Client
{
    public interface IWindowController
    {
        void CloseLoginWindow();
        void CloseMainWindow();
        void ShowLoginWindow();
        void ShowMainWindow();
    }
}
