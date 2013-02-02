using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BugTrackerService.Faults
{
    [DataContract]
    public class BadLoginFault
    {
        [DataMember]
        public string Reason {get; set;}
    }
}
