using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.ConfirmationResponse.Naesb;

namespace Nomination.BusinessLayer.Services.ConfirmationResponse.Event
{
    public interface IConfirmationResponseEventGet
    {
        //Nomination.Domain.Naesb.NaesbTransaction Invoke(string pipeline, string fileType, string trackingId);
        List<Domain.ConfirmationResponse.ConfirmationResponse> Invoke(string pipeline, string utility, DateTime gasday, string cycle);
        Domain.ConfirmationResponse.ConfirmationResponse Invoke(int id);
        Domain.ConfirmationResponse.ConfirmationResponse Invoke(NaesbConfirmationResponse obj);
        Domain.ConfirmationResponse.ConfirmationResponse Invoke(FileInfo file);
    }
}
