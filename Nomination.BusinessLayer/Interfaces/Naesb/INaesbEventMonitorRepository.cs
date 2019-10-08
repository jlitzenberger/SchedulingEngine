using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Interfaces.Naesb
{
    public interface INaesbEventMonitorRepository
    {
        NaesbEventMonitor Get(int id);
        //List<NaesbEventMonitor> GetEventMonitors(string cycle, string pipeline, string utility, string eventType, DateTime eventMonitorTime);
        List<NaesbEventMonitor> GetEventMonitors(DateTime eventMonitorTime);
        //void Update(int id, NaesbEventMonitor obj);
        void Update(int id, params object[] list);
    }
}
