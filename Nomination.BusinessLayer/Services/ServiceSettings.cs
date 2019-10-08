using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services
{
    public class ServiceSettings
    {
        public string UserId { get; set; }
        public string Environment { get; set; }

        public static ServiceSettings Get(string userId, string environment)
        {
            return new ServiceSettings()
            {
                UserId = userId,
                Environment = environment
            };
        }
    }
}
