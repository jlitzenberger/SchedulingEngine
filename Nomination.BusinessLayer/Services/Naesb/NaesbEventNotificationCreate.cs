using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventNotificationCreate : INaesbEventNotificationCreate
    {
        private readonly INaesbEventNotificationRepository _repository;

        public NaesbEventNotificationCreate(INaesbEventNotificationRepository repository)
        {
            _repository = repository;
        }
        public int Invoke(NaesbEventNotification obj)
        {
            return _repository.Create(obj);
        }
    }
}
