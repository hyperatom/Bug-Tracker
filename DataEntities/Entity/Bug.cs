using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace DataEntities.Entity
{
    [DataContract]
    public class Bug
    {
        [DataMember]
        [Range(0,9999)]
        public Int32 Id { get; set; }

        [DataMember]
        [StringLength(30)]
        [Required(AllowEmptyStrings=false)]
        public String Name { get; set; }

        [DataMember]
        [StringLength(200)]
        public String Description { get; set; }

        [DataMember]
        [StringLength(6)]
        [Required(AllowEmptyStrings = false)]
        public String Priority { get; set; }

        [DataMember]
        [StringLength(11)]
        [Required(AllowEmptyStrings = false)]
        public String Status { get; set; }

        [DataMember]
        [Required]
        public DateTime DateFound { get; set; }

        [DataMember]
        [Required]
        public DateTime LastModified { get; set; }

        [DataMember]
        public Boolean Fixed { get; set; }

        [DataMember]
        [Required]
        public virtual User CreatedBy { get; set; }

        [DataMember]
        [Required]
        public virtual Project Project { get; set; }
    }
}
