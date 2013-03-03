using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Threading;
using DataEntities.Repository;
using DataEntities.Entity;

namespace BugTrackerService.Security
{
    public class CustomPrincipal : IPrincipal
    {

        IIdentity _identity;
        string[] _roles;


        public CustomPrincipal(IIdentity identity)
        {
            _identity = identity;
        }

        
        public static CustomPrincipal Current
        {
            get
            {
                return Thread.CurrentPrincipal as CustomPrincipal;
            }
        }


        public IIdentity Identity
        {
            get { return _identity; }
        }

        
        public string[] Roles
        {
            get
            {
                EnsureRoles();
                return _roles;
            }
        }


        public bool IsInRole(string role)
        {
            EnsureRoles();

            return _roles.Contains(role);
        }


        protected virtual void EnsureRoles()
        {
            UserRepository repo = new UserRepository();
            ProjectRoleRepository projRoleRepo = new ProjectRoleRepository();

            User user = repo.GetAll().Where(p => p.Username == _identity.Name).Single();
            IList<Role> userRoles = new List<Role>();
            IList<ProjectRole> projRoles = projRoleRepo.GetAll().Where(p => p.User.Id == user.Id).ToList();
            
            foreach (ProjectRole projRole in projRoles)
            {
                if (!userRoles.Contains(projRole.Role))
                    userRoles.Add(projRole.Role);
            }

            _roles = new string[userRoles.Count()];

            for (int i = 0; i < _roles.Length; i++)
            {
                _roles[i] = userRoles[i].RoleName;
            }
        }

    }
}
