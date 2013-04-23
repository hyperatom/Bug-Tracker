using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities.Repository;
using DataEntities.Entity;


namespace BugTrackerService
{
    public static class BugActionLogger
    {

        public const int Create_Action = 1;
        public const int Update_Action = 2;
        public const int Delete_Action = 3;


        public static void LogEvent(Project project, User user, int actionConst, Bug bug)
        {
            var action = new BugActionRepository().GetAll().Where(p => p.Id == actionConst).SingleOrDefault();
            var proj = new ProjectRepository().GetAll().Where(p => p.Id == project.Id).SingleOrDefault();

            new BugActionLogRepository().Create(new BugActionLog 
                { Project = proj, Action = action, BugName = bug.Name, UserName = user.FirstName.Substring(0, 1) + "." + user.Surname });
        }

    }
}
