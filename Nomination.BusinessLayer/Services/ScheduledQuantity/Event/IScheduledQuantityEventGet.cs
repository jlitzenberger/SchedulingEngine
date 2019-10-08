using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.ScheduledQuantities.Naesb;

namespace Nomination.BusinessLayer.Services.ScheduledQuantity.Event
{
    public interface IScheduledQuantityEventGet
    {
        List<Domain.ScheduledQuantities.ScheduledQuantities> Invoke(string pipeline, string utility, DateTime gasday, string cycle);
        Domain.ScheduledQuantities.ScheduledQuantities Invoke(int id);
        Domain.ScheduledQuantities.ScheduledQuantities Invoke(NaesbScheduledQuantities obj);
        Domain.ScheduledQuantities.ScheduledQuantities Invoke(FileInfo file);
    }
}
