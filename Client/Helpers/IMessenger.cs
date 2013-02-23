using System;
namespace Client.Helpers
{
    public interface IMessenger
    {
        void NotifyColleagues(string message);
        void NotifyColleagues(string message, object parameter);
        void Register(string message, Action callback);
        void Register<T>(string message, Action<T> callback);
    }
}
