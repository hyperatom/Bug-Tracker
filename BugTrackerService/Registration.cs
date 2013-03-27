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

    /// <summary>
    /// Registration service allows unauthenticated users to access the
    /// registration method to create a new user and organisation.
    /// </summary>
    [ValidateDataAnnotationsBehavior]
    public class Registration : IRegistration
    {

        /// <summary>
        /// Creates a new system user.
        /// </summary>
        /// <param name="user">New user object.</param>
        public void Register(User user)
        {
            if (!UserExists(user.Username))
            {
                new UserRepository().Create(user);
            }
        }


        /// <summary>
        /// Checks if a user with the same username already exists
        /// in the database.
        /// </summary>
        /// <param name="username">The user to validate.</param>
        /// <returns></returns>
        public bool UserExists(String username)
        {
            UserRepository repo = new UserRepository();

            User userMatch = repo.GetAll().Where(p => p.Username == username).SingleOrDefault();

            if (userMatch != null)
                return true;

            return false;
        }

    }
}
