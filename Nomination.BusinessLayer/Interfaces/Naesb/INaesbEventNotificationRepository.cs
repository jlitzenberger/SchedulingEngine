using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Interfaces.Naesb
{
    public interface INaesbEventNotificationRepository
    {
        int Create(NaesbEventNotification obj);
        NaesbEventNotification Get(int id);
        List<NaesbEventNotification> GetEventNotificationsNotDelivered();
        //void Update(int id, NaesbEventNotification obj);
        void Update(int id, params object[] list);
    }
}
