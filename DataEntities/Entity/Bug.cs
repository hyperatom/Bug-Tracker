using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DataEntities.Entity
{
    [DataContract]
    public class Bug
    {
        [DataMember]
        public Int32 Id { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public String Priority { get; set; }
        [DataMember]
        public String Status { get; set; }
        [DataMember]
        public DateTime DateFound { get; set; }
        [DataMember]
        public DateTime LastModified { get; set; }
        [DataMember]
        public Boolean Fixed { get; set; }
        [DataMember]
        public virtual User CreatedBy { get; set; }
        [DataMember]
        public virtual Project Project { get; set; }
    }
}
