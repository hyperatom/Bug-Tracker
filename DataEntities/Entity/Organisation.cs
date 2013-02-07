using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DataEntities.Entity
{
    [DataContract]
    public class Organisation
    {
        [DataMember]
        [Range(1,9999)]
        public Int32 Id { get; set; }

        [DataMember]
        [Required(AllowEmptyStrings=false)]
        [StringLength(25)]
        public string Name { get; set; }
    }
}
