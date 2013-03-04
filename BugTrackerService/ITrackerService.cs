﻿using System;
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
        List<string> GetBugStatusList();

        [OperationContract]
        IList<Project> GetProjectsManagedBy(User user);

        [OperationContract]
        Project AddProject(Project project);

        [OperationContract]
        Project SaveProject(Project project);

        [OperationContract]
        void DeleteProject(Project project);
    }
}
