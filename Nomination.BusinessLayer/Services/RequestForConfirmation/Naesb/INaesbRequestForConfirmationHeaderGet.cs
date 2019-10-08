using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Naesb
{
    public interface INaesbRequestForConfirmationHeaderGet
    {
        Domain.RequestForConfirmation.Naesb.Header Invoke(Domain.RequestForConfirmation.RequestForConfirmation obj);
    }
}
