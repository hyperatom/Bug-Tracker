﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using BugTrackerService.Security;
using DataEntities.Entity;
using DataEntities.Model;
using DataEntities.Repository;
using DevTrends.WCFDataAnnotations;
using System.Security.Permissions;
using System.Threading;

namespace BugTrackerService
{

    [ValidateDataAnnotationsBehavior]
    public class TrackerService : ITrackerService
    {

        public List<Bug> GetAllBugs()
        {
            BugRepository repo = new BugRepository();
            List<Bug> bugList = repo.GetAll().ToList();

            return bugList;
        }


        public bool  DeleteBugs(IList<Bug> bugList)
        {
            foreach (Bug bug in bugList)
            {
                DeleteBug(bug);
            }

            return true;
        }


        /*[PrincipalPermission(SecurityAction.Demand, Role = "ADMIN")]*/


        public void DeleteBug(Bug bug)
        {
            BugRepository repo = new BugRepository();
            repo.Delete(bug);

            BugActionLogger.LogEvent(bug.Project, GetMyUser(), BugActionLogger.Delete_Action, bug);
        }


        public IList<Project> GetProjectsAssignedTo(User user)
        {
            ProjectRoleRepository projRoleRepo = new ProjectRoleRepository();

            return projRoleRepo.GetAll().Where(p => p.User.Id == user.Id)
                .Select(c => c.Project).Distinct().ToList();
        }


        public List<Bug> GetBugsByProject(Project project)
        {
            BugRepository bugRepo = new BugRepository();

            List<Bug> bugList = bugRepo.GetAll().Where(x => x.Project.Id == project.Id).ToList();

            return bugList;
        }


        public Bug SaveBug(Bug bug)
        {
            bug.LastModified = DateTime.Now;

            BugRepository bugRepo = new BugRepository();

            var savedbug = bugRepo.Update(bug);

            BugActionLogger.LogEvent(bug.Project, GetMyUser(), BugActionLogger.Update_Action, savedbug);

            return bug;
        }


        public Bug AddBug(Bug bug)
        {
            bug.DateFound = DateTime.Now;
            bug.LastModified = DateTime.Now;
            
            var savedbug = new BugRepository().Create(bug);

            BugActionLogger.LogEvent(bug.Project, GetMyUser(), BugActionLogger.Create_Action, savedbug);

            return savedbug;
        }


        public User GetMyUser()
        {
            UserRepository repo = new UserRepository();

            return (User)repo.GetAll().Where(p => p.Username == CustomPrincipal.Current.Identity.Name).SingleOrDefault();
        }


        public List<string> GetBugPriorityList()
        {
            return new List<string>() 
            {
                "Low",
                "Medium",
                "High"
            };
        }


        public List<User> GetUsersByProject(Project proj)
        {
            ProjectRoleRepository projRoleRepo = new ProjectRoleRepository();

            List<User> userList = new List<User>();
            List<ProjectRole> projRoleList = projRoleRepo.GetAll().Where(p => p.Project.Id == proj.Id).ToList();

            foreach (ProjectRole projRole in projRoleList)
            {
                if (!userList.Contains(projRole.User))
                    userList.Add(projRole.User);
            }

            return userList;
        }


        public List<string> GetBugStatusList()
        {
            return new List<string>()
            {
                "Open",
                "In Progress",
                "Closed"
            };
        }



        public IList<Project> GetProjectsManagedBy(User user)
        {
            ProjectRoleRepository projRoleRepo = new ProjectRoleRepository();
            RoleRepository roleRepo = new RoleRepository();

            Role manager = roleRepo.GetAll().Where(r => r.RoleName == "Project Manager").SingleOrDefault();

            return projRoleRepo.GetAll().Where(p => p.User.Id == user.Id && p.Role.Id == manager.Id).Select(c => c.Project).Distinct().ToList();
        }


        public Project AddProject(Project project)
        {
            ProjectRepository projRepo = new ProjectRepository();
            ProjectRoleRepository projRoleRepo = new ProjectRoleRepository();
            RoleRepository roleRepo = new RoleRepository();

            Project addedProj = projRepo.Create(project);

            int ManagerId = roleRepo.GetAll().Where(x => x.RoleName == "Project Manager").Select(c => c.Id).SingleOrDefault();
            int CurrentUserId = GetMyUser().Id;

            // Make the user who added the project, a project manager
            projRoleRepo.Create(new ProjectRole { ProjectId = addedProj.Id, RoleId = ManagerId, UserId = CurrentUserId });

            return addedProj;
        }


        public Project SaveProject(Project project)
        {
            ProjectRepository projRepo = new ProjectRepository();

            return projRepo.Update(project);
        }


        public void DeleteProject(Project project)
        {
            ProjectRepository projRepo = new ProjectRepository();

            projRepo.Delete(project);
        }


        public IList<Project> GetAllProjectsByUser(User user)
        {
            ProjectRoleRepository projRoleRepo = new ProjectRoleRepository();

            return projRoleRepo.GetAll().Where(p => p.User.Id == user.Id).Select(p => p.Project).Distinct().ToList();
        }


        public void LeaveProject(Project project, User user)
        {
            ProjectRoleRepository projRoleRepo = new ProjectRoleRepository();

            var associations = projRoleRepo.GetAll().Where(p => p.User.Id == user.Id && p.Project.Id == project.Id);

            foreach (var ass in associations)
            {
                projRoleRepo.Delete(ass);
            }
        }


        public IList<User> GetAssignedUsersByProject(Project proj)
        {
            ProjectRoleRepository projRoleRepo = new ProjectRoleRepository();
            RoleRepository roleRepo = new RoleRepository();

            Role projMgr = roleRepo.GetAll().Where(p => p.RoleName == "Project Manager").SingleOrDefault();

            return projRoleRepo.GetAll().Where(p => p.Project.Id == proj.Id && p.Role.Id != projMgr.Id)
                                        .Select(p => p.User).Distinct().ToList();
        }


        public void RequestProjectAssignment(String code, User user, Role role)
        {
            Project proj = new ProjectRepository().GetAll().Where(p => p.Code == code).SingleOrDefault();

            if (proj == null)
                throw new FaultException("A project with this code does not exist.");

            var existingRequest = new UserProjectSignupRepository().GetAll().Where(
                p => p.ProjectId == proj.Id && p.UserId == user.Id).SingleOrDefault();

            if (existingRequest != null)
            {
                new UserProjectSignupRepository().Delete(existingRequest);
            }

            UserProjectSignup ass = new UserProjectSignup { ProjectId = proj.Id, UserId = user.Id, RoleId = role.Id };

            new UserProjectSignupRepository().Create(ass);
        }


        public IList<Role> GetAllRoles()
        {
            RoleRepository roleRepo = new RoleRepository();

            return roleRepo.GetAll();
        }

    
        public Project GetProjectByCode(String projectCode)
        {
            return new ProjectRepository().GetAll().Where(p => p.Code == projectCode).SingleOrDefault();
        }


        public IList<User> GetUsersPendingProjectJoin(Project project)
        {
            return new UserProjectSignupRepository().GetAll()
                .Where(p => p.ProjectId == project.Id).Select(p => p.User).Distinct().ToList();
        }


        [PrincipalPermission(SecurityAction.Demand, Role = "Project Manager")]
        public void AcceptUserOnProject(User user, Project project, String rolename)
        {
            UserProjectSignupRepository signUpRepo = new UserProjectSignupRepository();

            Role acceptedRole = new RoleRepository().GetAll().Where(p => p.RoleName == rolename).SingleOrDefault();

            UserProjectSignup request = signUpRepo.GetAll()
                .Where(p => p.UserId == user.Id && p.ProjectId == project.Id).SingleOrDefault();

            new ProjectRoleRepository().Create
                (new ProjectRole { UserId = request.UserId, ProjectId = request.ProjectId, RoleId = acceptedRole.Id });

            signUpRepo.Delete(request);
        }


        public void RejectUserFromProject(User user, Project project)
        {
            UserProjectSignupRepository signUpRepo = new UserProjectSignupRepository();

            UserProjectSignup request = signUpRepo.GetAll()
                .Where(p => p.UserId == user.Id && p.ProjectId == project.Id).SingleOrDefault();

            signUpRepo.Delete(request);            
        }


        public bool IsValidProjectCode(String code)
        {
            return new ProjectRepository().GetAll().Select(p => p.Code).Contains(code);
        }


        public IList<User> GetManagerUsersByProject(Project proj)
        {
            Role projMgr = new RoleRepository().GetAll().Where(p => p.RoleName == "Project Manager").SingleOrDefault();

            return new ProjectRoleRepository().GetAll()
                .Where(p => p.ProjectId == proj.Id && p.RoleId == projMgr.Id).Select(p => p.User).Distinct().ToList();
        }


        public int CountBugsInProject(Project project)
        {
            return new BugRepository().GetAll().Where(p => p.Project.Id == project.Id).Count();
        }


        public IList<Bug> SearchAllProjectBugsAttributes(Project project, string searchText)
        {
            searchText = searchText.Trim();

           // if (searchText == null || searchText == "")
             //   return GetBugsByProject(project);

            IList<int> associatedUserIds = new UserRepository().GetAll().FullTextSearch(searchText, true).Select(p => p.Id).ToList();
            IList<int> fullTextSearch    = new BugRepository() .GetAll().FullTextSearch(searchText, true).Select(p => p.Id).ToList();

            if (associatedUserIds.Count == 0 && fullTextSearch.Count == 0)
                return new List<Bug>();

            int id = 0;
            DateTime date = DateTime.MinValue;
            
            try { id = Int32.Parse(searchText); } 
            catch (Exception e) { Console.WriteLine(e.Message); }

            try { date = DateTime.Parse(searchText); }
            catch (Exception e) { Console.WriteLine(e.Message); }

            
            return new BugRepository().GetAll()
                    .Where(p => associatedUserIds.Contains(p.AssignedUser.Id) ||
                                associatedUserIds.Contains(p.CreatedBy.Id) ||
                                p.Id == id ||
                                (p.DateFound.Year == date.Year && p.DateFound.Month == date.Month && p.DateFound.Day == date.Day) ||
                                (p.LastModified.Year == date.Year && p.LastModified.Month == date.Month && p.LastModified.Day == date.Day) ||
                                fullTextSearch.Contains(p.Id)).Where(p => p.Project.Id == project.Id).ToList();

        }



        public int GetNumberOfBugsAssignedToUserInProject(Project project, User user)
        {
            return new BugRepository().GetAll().Where(p => p.Project.Id == project.Id && p.AssignedUser.Id == user.Id).Count();
        }


        public IList<Bug> GetBugsAssignedToProjectAndUser(Project project, User user)
        {
            return new BugRepository().GetAll().Where(p => p.Project.Id == project.Id && p.AssignedUser.Id == user.Id).ToList();
        }


        public void SaveUserCredentials(User user)
        {
            if (CustomPrincipal.Current.Identity.Name == user.Username)
            {
                new UserRepository().Update(user);
            }
        }


        public bool UserExists(String user)
        {
            if (GetMyUser().Username == user)
                return false;

            return new RegistrationService().UserExists(user);
        }


        public IList<Bug> GetOpenBugsInProject(Project project)
        {
            return new BugRepository().GetAll().Where(p => p.Project.Id == project.Id && p.Status == "Open").ToList();
        }

        public IList<Bug> GetBugsInProgressFromProject(Project project)
        {
            return new BugRepository().GetAll().Where(p => p.Project.Id == project.Id && p.Status == "In Progress").ToList();
        }

        public IList<Bug> GetClosedBugsInProject(Project project)
        {
            return new BugRepository().GetAll().Where(p => p.Project.Id == project.Id && p.Status == "Closed").ToList();
        }


        public IList<BugActionLog> GetAllBugActionLogsInProject(Project project)
        {
            return new BugActionLogRepository().GetAll().Where(p => p.Project.Id == project.Id).OrderByDescending(p => p.Date).ToList();
        }


        public bool ProjectCodeExists(string code)
        {
            return (new ProjectRepository().GetAll().Where(p => p.Code == code).SingleOrDefault() != null);
        }


        public bool ProjectCodeExistsExcludingProject(Project proj)
        {
            return (new ProjectRepository().GetAll().Where(p => p.Id != proj.Id && p.Code == proj.Code).SingleOrDefault() != null);
        }


        public string GetUsersRequestedRoleForProject(User user, Project project)
        {
            return new UserProjectSignupRepository().GetAll()
                .Where(p => p.ProjectId == project.Id && user.Id == p.UserId).Select(p => p.Role.RoleName).SingleOrDefault();
        }
    }
}
