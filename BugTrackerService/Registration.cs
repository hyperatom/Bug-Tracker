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
        /// Creates a new system user with the role of project manager
        /// and assigns the newly created organisation.
        /// </summary>
        /// <param name="user">New user object containing new organisation.</param>
        public void Register(User user)
        {
            UserRepository userRepo = new UserRepository();
            OrganisationRepository orgRepo = new OrganisationRepository();
            RoleRepository roleRepo = new RoleRepository();

            if (UserExists(user))
                throw new FaultException("A user with this username already exists.");

            Role role = roleRepo.GetAll().Where(p => p.RoleName == "Project Manager").SingleOrDefault();

            user.Organisation = orgRepo.Create(user.Organisation);
            user.Roles = new List<Role>() { role };
            userRepo.Create(user);
        }


        /// <summary>
        /// Checks if a user with the same username already exists
        /// in the database.
        /// </summary>
        /// <param name="user">The user to validate.</param>
        /// <returns></returns>
        private bool UserExists(User user)
        {
            UserRepository repo = new UserRepository();

            User userMatch = repo.GetAll().Where(p => p.Username == user.Username).SingleOrDefault();

            if (userMatch != null)
                return true;

            return false;
        }

    }
}
