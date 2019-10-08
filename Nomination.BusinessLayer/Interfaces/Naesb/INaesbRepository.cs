using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.ConfirmationResponse;
using Nomination.Domain.RequestForConfirmation;

namespace Nomination.BusinessLayer.Interfaces.Naesb
{
    public interface INaesbRepository
    {
        Nomination.Domain.RequestForConfirmation.Naesb.Header GetNaesbRequestForConfirmationHeader(RequestForConfirmation obj);
        Nomination.Domain.QuickResponse.Naesb.Header GetNaesbQuickResponseHeader(ConfirmationResponse obj);
    }
}
