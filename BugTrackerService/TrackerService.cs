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

        public bool Login()
        {
            return true;
        }


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


        public List<Project> GetMyProjects()
        {
            UserRepository repo = new UserRepository();
            ProjectRepository projRepo = new ProjectRepository();
            
            string identity = CustomPrincipal.Current.Identity.Name;
            User currentUser = repo.GetAll().Where(x => x.Username == identity).FirstOrDefault();

            return currentUser.Projects.ToList();
        }


        public List<Bug> GetBugsByProject(Project project)
        {
            BugRepository bugRepo = new BugRepository();

            List<Bug> bugList = bugRepo.GetAll().Where(x => x.Project.Id == project.Id).ToList();

            return bugList;
        }


        public void SaveBug(Bug bug)
        {
            BugRepository bugRepo = new BugRepository();

            bugRepo.Update(bug);
        }


        public Bug AddBug(Bug bug)
        {
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
            UserRepository userRep = new UserRepository();

            return userRep.GetAll().Where(p => p.Projects.Contains(proj)).ToList();
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

    }
}
