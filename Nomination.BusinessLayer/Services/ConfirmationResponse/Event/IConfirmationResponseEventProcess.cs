using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.ConfirmationResponse.Event
{
    public interface IConfirmationResponseEventProcess
    {
        void Invoke(string pipeline, string utility, DateTime gasDay, string cycle);
    }
}
