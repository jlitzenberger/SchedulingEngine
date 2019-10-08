using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Naesb
{
    public interface INaesbRequestForConfirmationCreate
    {
        //int Invoke(NaesbEventProcess obj);
        int Invoke(DateTime processStart, DateTime gasDay, string pipeline, string utility, string cycle);
        int Invoke(DateTime processStart, string fileName, Domain.RequestForConfirmation.RequestForConfirmation obj);
    }
}
