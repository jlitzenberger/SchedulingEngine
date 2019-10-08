using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Common.Utilities.Email.Services
{
    public interface IEmailService
    {
        void Send(Models.Email email);
    }
}
