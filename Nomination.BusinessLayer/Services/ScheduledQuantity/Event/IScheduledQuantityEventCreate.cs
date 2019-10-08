using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.ScheduledQuantity.Event
{
    public interface IScheduledQuantityEventCreate
    {
        int Invoke(Nomination.Domain.ScheduledQuantities.ScheduledQuantities obj);
    }
}
