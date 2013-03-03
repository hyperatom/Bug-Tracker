using System.ServiceModel;
using Client.ServiceReference;

namespace Client.Factories
{
    /// <summary>
    /// Provides an interface to the service factory.
    /// </summary>
    public interface IServiceFactory
    {
        ClientBase<ITrackerService> CreateService(string username, string password);
    }
}
