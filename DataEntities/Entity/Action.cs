using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DataEntities.Entity
{
    [DataContract]
    public class Action
    {
        [DataMember]
        [Range(0, 99)]
        public Int32 Id { get; set; }

        [DataMember]
        [StringLength(20)]
        [Required]
        public String ActionName { get; set; }
    }
}

