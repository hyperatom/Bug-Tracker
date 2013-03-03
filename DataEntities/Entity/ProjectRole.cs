using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DataEntities.Entity
{
    [DataContract]
    public class ProjectRole
    {

        [DataMember]
        public Int32 UserId { get; set; }

        [DataMember]
        public Int32 ProjectId { get; set; }

        [DataMember]
        public Int32 RoleId { get; set; }

        [DataMember]
        public virtual Project Project { get; set; }

        [DataMember]
        public virtual User User { get; set; }

        [DataMember]
        public virtual Role Role { get; set; }

    }
}
