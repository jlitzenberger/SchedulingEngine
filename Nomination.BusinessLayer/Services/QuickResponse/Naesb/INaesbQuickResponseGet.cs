using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.QuickResponse.Naesb;

namespace Nomination.BusinessLayer.Services.QuickResponse.Naesb
{
    public interface INaesbQuickResponseGet
    {
        NaesbQuickResponse Invoke(Domain.ConfirmationResponse.ConfirmationResponse confirmationResponse);
    }
}
