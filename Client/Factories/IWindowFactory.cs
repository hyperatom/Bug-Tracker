
namespace Client.Factories
{
    /// <summary>
    /// Provides an interface to the window factory.
    /// </summary>
    public interface IWindowFactory
    {
        ILoginWindow CreateLoginWindow();
        IMainWindow CreateMainWindow();
        IRegistrationWindow CreateRegistrationWindow();
    }
}
