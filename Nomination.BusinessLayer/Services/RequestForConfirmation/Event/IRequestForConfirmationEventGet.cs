using System;
using System.Collections.Generic;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Event
{
    public interface IRequestForConfirmationEventGet
    {
        List<Domain.RequestForConfirmation.RequestForConfirmation> Invoke(string pipeline, string utility, DateTime gasday, string cycle);
        Domain.RequestForConfirmation.RequestForConfirmation Invoke(int id);
    }
}
