using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataEntities.Entity;
using DataEntities.Repository;
using System.Security.Permissions;
using BugTrackerService.Security;
using System.Threading;
using BugTrackerService.Faults;
using DevTrends.WCFDataAnnotations;

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
                BugRepository repo = new BugRepository();
                repo.Delete(bug);
            }

            return true;
        }


        /*[PrincipalPermission(SecurityAction.Demand, Role = "ADMIN")]*/


        public void DeleteBug(Bug bug)
        {
            BugRepository repo = new BugRepository();
            repo.Delete(bug);
        }


        public IList<Project> GetProjectsAssignedTo(User user)
        {
            ProjectRoleRepository projRoleRepo = new ProjectRoleRepository();
            RoleRepository roleRepo = new RoleRepository();

            int projManager = roleRepo.GetAll().Where(p => p.RoleName == "Project Manager")
                .Select(p => p.Id).SingleOrDefault();

            return projRoleRepo.GetAll().Where(p => p.User.Id == user.Id && p.RoleId != projManager)
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

            return bugRepo.Update(bug);
        }


        public Bug AddBug(Bug bug)
        {
            bug.DateFound = DateTime.Now;
            bug.LastModified = DateTime.Now;

            BugRepository bugRepo = new BugRepository();

            return bugRepo.Create(bug);
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


        public void AcceptUserOnProject(User user, Project project)
        {
            UserProjectSignupRepository signUpRepo = new UserProjectSignupRepository();

            UserProjectSignup request = signUpRepo.GetAll()
                .Where(p => p.UserId == user.Id && p.ProjectId == project.Id).SingleOrDefault();

            new ProjectRoleRepository().Create
                (new ProjectRole { UserId = request.UserId, ProjectId = request.ProjectId, RoleId = request.RoleId });

            signUpRepo.Delete(request);
        }


        public void RejectUserFromProject(User user, Project project)
        {
            UserProjectSignupRepository signUpRepo = new UserProjectSignupRepository();

            UserProjectSignup request = signUpRepo.GetAll()
                .Where(p => p.UserId == user.Id && p.ProjectId == project.Id).SingleOrDefault();

            signUpRepo.Delete(request);            
        }

    }
}
