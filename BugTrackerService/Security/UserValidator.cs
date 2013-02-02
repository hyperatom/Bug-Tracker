using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using DataEntities.Repository;

namespace BugTrackerService.Security
{

    public class UserValidator : UserNamePasswordValidator
    {

        public override void Validate(string userName, string password)
        {

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                throw new SecurityTokenException("Username and password required.");
            }
            
            if (!(IsValidUser(userName, password)))
            {
                throw new FaultException(string.Format("Wrong username ({0}) or password ", userName));
            }
            
        }


        private bool IsValidUser(string userName, string password)
        {
            UserRepository repo = new UserRepository();

            int count = repo.GetAll().Where(p => p.Username == userName && p.Password == password).Count();

            if (count == 1) { return true; }

            return false;
        }

    }
}
