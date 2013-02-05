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
    public interface ITrackerService
    {
        [OperationContract]
        //[FaultContract(typeof(BadLoginFault))]
        bool Login();

        [OperationContract]
        List<Bug> GetAllBugs();

        [OperationContract]
        bool DeleteBugs(IList<Bug> bugList);

        [OperationContract]
        void DeleteBug(Bug bug);

        [OperationContract]
        List<Project> GetMyProjects();

        [OperationContract]
        List<Bug> GetBugsByProject(Project project);

        [OperationContract]
        void SaveBug(Bug bug);

        [OperationContract]
        Bug AddBug(Bug bug);

        [OperationContract]
        User GetMyUser();
    }
}
