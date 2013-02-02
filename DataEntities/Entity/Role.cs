using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DataEntities.Entity
{
    [DataContract]
    public class Role
    {

        [DataMember]
        public Int32 Id { get; set; }
        [DataMember]
        public string RoleName { get; set; }

    }
}
