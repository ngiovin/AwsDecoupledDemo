using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageLayer.Models
{
    public class DatabaseSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
