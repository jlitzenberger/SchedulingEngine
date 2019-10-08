using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.QuickResponse.Naesb
{
    public interface INaesbQuickResponseHeaderGet
    {
        Domain.QuickResponse.Naesb.Header Invoke(Domain.ConfirmationResponse.ConfirmationResponse obj);
    }
}
