using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.RequestForConfirmation;

namespace Nomination.BusinessLayer.Interfaces
{
    public interface IRequestForConfirmationRepository
    {
        RequestForConfirmation Get(string pipeline, string utility, DateTime gasDay, string cycle);
        //RequestForConfirmation Get(int id);
        //int Create(RequestForConfirmation obj, string userId);
    }
}
