using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DataEntities.Entity
{
    [DataContract]
    public class Project
    {
        [DataMember]
        public Int32 Id { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Description { get; set; }
    }
}
