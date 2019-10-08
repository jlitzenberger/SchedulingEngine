using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventMonitorGet : INaesbEventMonitorGet
    {
        private readonly INaesbEventMonitorRepository _repository;
        public NaesbEventMonitorGet(INaesbEventMonitorRepository repository)
        {
            _repository = repository;
        }

        public NaesbEventMonitor Invoke(int id)
        {
            return _repository.Get(id);
        }
    }
}
