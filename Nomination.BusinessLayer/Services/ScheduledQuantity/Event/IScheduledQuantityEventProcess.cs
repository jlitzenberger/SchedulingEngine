using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.ScheduledQuantity.Event
{
    public interface IScheduledQuantityEventProcess
    {
        void Invoke(string pipeline, string utility, DateTime gasDay, string cycle);
    }
}
