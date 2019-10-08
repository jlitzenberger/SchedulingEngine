using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventMonitorUpdate : INaesbEventMonitorUpdate
    {
        private readonly INaesbEventMonitorRepository _repository;
        public NaesbEventMonitorUpdate(INaesbEventMonitorRepository repository)
        {
            _repository = repository;
        }
        public void Invoke(int id, DateTime lastCheckedTime)
        {
            _repository.Update(id, new KeyValuePair<string, DateTime?>("LastCheckedTime", lastCheckedTime));
        }
    }
}
