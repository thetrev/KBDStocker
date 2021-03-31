using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBDStocker.Models
{
    public class Settings
    {
        public string[] Urls { get; set; }

        public int? WaitTimeInSeconds { get; set; }
        
        public string DiscordApiKey { get; set; }
        
        public string UserToPing { get; set; }
    }
}
