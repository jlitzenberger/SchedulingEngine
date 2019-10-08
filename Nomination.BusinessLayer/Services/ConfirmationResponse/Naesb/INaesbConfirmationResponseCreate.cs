using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.ConfirmationResponse.Naesb;

namespace Nomination.BusinessLayer.Services.ConfirmationResponse.Naesb
{
    public interface INaesbConfirmationResponseCreate
    {
        //int Invoke(NaesbEventProcess obj);
        int Invoke(DateTime processStart, NaesbConfirmationResponse naesbObj);
        int Invoke(DateTime processStart, FileInfo file);
    }
}
