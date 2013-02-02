using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DataEntities.Entity
{
    [DataContract]
    public class User
    {
        [DataMember]
        public Int32 Id { get; set; }
        [DataMember]
        public String FirstName { get; set; }
        [DataMember]
        public String Surname { get; set; }
        [DataMember]
        public String Username { get; set; }
        [DataMember]
        public String Password { get; set; }
        [DataMember]
        public IList<Project> Projects { get; set; }
        [DataMember]
        public IList<Role> Roles { get; set; }
    }
}
