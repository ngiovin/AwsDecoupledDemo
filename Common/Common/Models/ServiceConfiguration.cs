using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ServiceConfiguration
    {
        public AWSSQS AWSSQS { get; set; }
        public string Type { get; set; }
    }
    public class AWSSQS
    {
        public string QueueUrlGetUsers { get; set; }
        public string QueueUrlAddUser { get; set; }
        
    }
}
