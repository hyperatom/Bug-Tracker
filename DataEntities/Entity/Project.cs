using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DataEntities.Entity
{
    [DataContract]
    public class Project
    {
        [DataMember]
        [Range(1, 9999)]
        public Int32 Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(40)]
        public String Name { get; set; }

        [DataMember]
        [StringLength(200)]
        public String Description { get; set; }

    }
}
