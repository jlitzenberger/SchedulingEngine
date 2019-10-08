using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventNotificationUpdate : INaesbEventNotificationUpdate
    {
        private readonly INaesbEventNotificationRepository _repository;
        public NaesbEventNotificationUpdate(INaesbEventNotificationRepository repository)
        {
            _repository = repository;
        }
        public void Invoke(int id, DateTime processedTime)
        {
            _repository.Update(id, new KeyValuePair<string, DateTime?>("ProcessedTime", processedTime));
        }
    }
}
