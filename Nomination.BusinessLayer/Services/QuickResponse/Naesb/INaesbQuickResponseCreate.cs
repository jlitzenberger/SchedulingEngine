using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.QuickResponse.Naesb
{
    public interface INaesbQuickResponseCreate
    {
        //int Invoke(DateTime processStart, DateTime gasDay, string cycle, string fileName, NaesbQuickResponse nqr);
        int Invoke(DateTime processStart, string fileName, Domain.ConfirmationResponse.ConfirmationResponse cr);
    }
}