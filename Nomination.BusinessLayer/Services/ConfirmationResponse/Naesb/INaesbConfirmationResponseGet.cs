using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.ConfirmationResponse.Naesb;

namespace Nomination.BusinessLayer.Services.ConfirmationResponse.Naesb
{
    public interface INaesbConfirmationResponseGet
    {
        List<NaesbConfirmationResponse> Invoke(DateTime gasday, string pipeline, string utility, string cycle);
        NaesbConfirmationResponse Invoke(int id);
    }
}
