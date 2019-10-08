using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Common.Utilities.Email.Settings
{
    public interface IEmailSettings
    {
        string Host { get; set; }
        int Port { get; set; }
    }
}
