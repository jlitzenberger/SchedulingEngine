using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation
{
    public interface IRequestForConfirmationGet
    {
        Nomination.Domain.RequestForConfirmation.RequestForConfirmation Invoke(string pipeline, string utility, DateTime gasDay, string cycle);
        //Nomination.Domain.RequestForConfirmation.RequestForConfirmation Invoke(int id);
    }
}
