using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventNotificationGetNotDelivered : INaesbEventNotificationGetNotDelivered
    {
        private readonly INaesbEventNotificationRepository _repository;
        public NaesbEventNotificationGetNotDelivered(INaesbEventNotificationRepository repository)
        {
            _repository = repository;
        }

        public List<NaesbEventNotification> Invoke()
        {
            return _repository.GetEventNotificationsNotDelivered();
        }
    }
}
