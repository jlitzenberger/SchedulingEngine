using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventMonitorGetEventMonitors : INaesbEventMonitorGetEventMonitors
    {
        private readonly INaesbEventMonitorRepository _repository;
        public NaesbEventMonitorGetEventMonitors(INaesbEventMonitorRepository repository)
        {
            _repository = repository;
        }

        public List<NaesbEventMonitor> Invoke(DateTime eventMonitorTime)
        {
            return _repository.GetEventMonitors(eventMonitorTime);
        }
    }
}
