using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataEntities.Entity;
using BugTrackerService.Faults;

namespace BugTrackerService
{

    [ServiceContract]
    public interface IRegistration
    {
        [OperationContract]
        void Register(User user);

        [OperationContract]
        bool UserExists(String username);
    }

}

