using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DataEntities.Entity
{
    [DataContract]
    public class Role
    {

        [DataMember]
        [Range(0,9999)]
        public Int32 Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(20)]
        public string RoleName { get; set; }

    }
}
