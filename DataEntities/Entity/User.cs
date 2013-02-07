using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DataEntities.Entity
{
    [DataContract]
    public class User
    {
        [DataMember]
        [Range(1, 9999)]
        public Int32 Id { get; set; }

        [DataMember]
        [StringLength(25)]
        [Required]
        public String FirstName { get; set; }

        [DataMember]
        [Required]
        [StringLength(40)]
        public String Surname { get; set; }

        [DataMember]
        [Required]
        [StringLength(254)]
        public String Username { get; set; }

        [DataMember]
        [Required]
        [StringLength(60)]
        public String Password { get; set; }

        [DataMember]
        [Required]
        public IList<Project> Projects { get; set; }

        [DataMember]
        [Required]
        public IList<Role> Roles { get; set; }

        [DataMember]
        [Required]
        public Organisation Organisation { get; set; }
    }
}
