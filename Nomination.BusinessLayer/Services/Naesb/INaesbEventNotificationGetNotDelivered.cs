using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public interface INaesbEventNotificationGetNotDelivered
    {
        List<Nomination.Domain.Naesb.NaesbEventNotification> Invoke();
    }
}
