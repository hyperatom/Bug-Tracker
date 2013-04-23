using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DataEntities.Entity
{
    [DataContract]
    public class BugActionLog
    {
        [DataMember]
        [Range(0, 99999)]
        public Int32 Id { get; set; }

        [DataMember]
        [Required]
        public virtual BugAction Action { get; set; }

        [DataMember]
        [Required(AllowEmptyStrings = false)]
        public String BugName { get; set; }

        [DataMember]
        [Required]
        public DateTime Date { get; set; }

        [DataMember]
        [Required]
        public String UserName { get; set; }

        [DataMember]
        [Required]
        public Project Project { get; set; }
    }
}
