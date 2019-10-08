using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.RequestForConfirmation.Naesb;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Naesb
{
    public interface INaesbRequestForConfirmationGet
    {
        NaesbRequestForConfirmation Invoke(Domain.RequestForConfirmation.RequestForConfirmation obj);
        List<NaesbRequestForConfirmation> Invoke(DateTime gasday, string pipeline, string utility, string cycle);
        NaesbRequestForConfirmation Invoke(int id);
    }
}
