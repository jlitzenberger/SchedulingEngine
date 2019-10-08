using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.ConfirmationResponse;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Interfaces
{
    public interface IConfirmationResponseEventRepository
    {
        List<ConfirmationResponse> Get(string pipeline, string utility, DateTime gasday, string cycle);
        ConfirmationResponse Get(int id);
        NaesbTransaction Get(string pipeline, string fileType, string trackingId);
        int Create(ConfirmationResponse obj, string userId);
        void Process(string pipeline, string utility, DateTime gasDay, string cycle);
    }
}
