﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using DataEntities.Entity;

namespace DataEntities.Entity
{

    public class WcfEntityContext : ObjectContext
    {

        public ObjectSet<User> Users { get; set; }
        public ObjectSet<Bug> Bugs { get; set; }
        public ObjectSet<Project> Projects { get; set; }
        public ObjectSet<Role> Roles { get; set; }
        public ObjectSet<ProjectRole> ProjectRole { get; set; }
        public ObjectSet<UserProjectSignup> UserProjectSignup { get; set; }
        public ObjectSet<BugActionLog> BugActionLogs { get; set; }
        public ObjectSet<BugAction> Actions { get; set; }

        public WcfEntityContext() : this("name=DataEntities") { }

        public WcfEntityContext(string connectionString)
            : base(connectionString, "DataEntities")
        {
            ProjectRole = CreateObjectSet<ProjectRole>();
            UserProjectSignup = CreateObjectSet<UserProjectSignup>();
            Users = CreateObjectSet<User>();
            Bugs = CreateObjectSet<Bug>();
            Projects = CreateObjectSet<Project>();
            Roles = CreateObjectSet<Role>();
            BugActionLogs = CreateObjectSet<BugActionLog>();
            Actions = CreateObjectSet<BugAction>();

            //Turned off to avoid problems with serialization
            ContextOptions.ProxyCreationEnabled = false;
            ContextOptions.LazyLoadingEnabled = false;
        }


        public void SetModified(object entity)
        {
            ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
        }


        public void AttachModify(string entitySetName, object entity)
        {
            AttachTo(entitySetName, entity);
            SetModified(entity);
        }


        public void AttachModify(string entitySetName, IEnumerable<Object> entities)
        {
            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    AttachModify(entitySetName, entity);
                }
            }
        }

    }
    
}
