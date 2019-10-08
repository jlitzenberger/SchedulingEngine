using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.ConfirmationResponse.Event
{
    public interface IConfirmationResponseEventCreate
    {
        int Invoke(Nomination.Domain.ConfirmationResponse.ConfirmationResponse obj);

        //int Invoke(DateTime processStart, System.IO.FileInfo file, string userId);
    }
}
