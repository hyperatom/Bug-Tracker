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
        List<Bug> GetAllBugs();

        [OperationContract]
        bool DeleteBugs(IList<Bug> bugList);

        [OperationContract]
        void DeleteBug(Bug bug);

        [OperationContract]
        IList<Project> GetProjectsAssignedTo(User user);

        [OperationContract]
        IList<Project> GetAllProjectsByUser(User user);

        [OperationContract]
        List<Bug> GetBugsByProject(Project project);

        [OperationContract]
        Bug SaveBug(Bug bug);

        [OperationContract]
        Bug AddBug(Bug bug);

        [OperationContract]
        User GetMyUser();

        [OperationContract]
        List<String> GetBugPriorityList();

        [OperationContract]
        List<User> GetUsersByProject(Project proj);

        [OperationContract]
        IList<User> GetAssignedUsersByProject(Project proj);

        [OperationContract]
        List<string> GetBugStatusList();

        [OperationContract]
        IList<Project> GetProjectsManagedBy(User user);

        [OperationContract]
        Project AddProject(Project project);

        [OperationContract]
        Project SaveProject(Project project);

        [OperationContract]
        void DeleteProject(Project project);

        [OperationContract]
        void LeaveProject(Project project, User user);

        [OperationContract]
        void RequestProjectAssignment(String code, User user, Role role);

        [OperationContract]
        IList<Role> GetAllRoles();

        [OperationContract]
        Project GetProjectByCode(String projectCode);

        [OperationContract]
        IList<User> GetUsersPendingProjectJoin(Project project);

        [OperationContract]
        void AcceptUserOnProject(User user, Project project);

        [OperationContract]
        void RejectUserFromProject(User user, Project project);
    }
}
