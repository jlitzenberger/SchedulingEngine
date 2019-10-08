using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;
using Nomination.Domain.ScheduledQuantities;

namespace Nomination.BusinessLayer.Interfaces
{
    public interface IScheduledQuantityEventRepository
    {
        List<ScheduledQuantities> Get(string pipeline, string utility, DateTime gasday, string cycle);
        ScheduledQuantities Get(int id);
        NaesbTransaction Get(string pipeline, string fileType, string trackingId);
        int Create(ScheduledQuantities obj, string userId);
        void Process(string pipeline, string utility, DateTime gasDay, string cycle);
    }
}
