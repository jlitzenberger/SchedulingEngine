using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.Domain.Naesb;
using Nomination.Persistence.Common;
using Nomination.Persistence.Shared;

namespace Nomination.Persistence.Naesb
{
    public class NaesbEventMonitorRepository : Repository<tb_naesb_event_monitor>, INaesbEventMonitorRepository
    {
        public NaesbEventMonitor Get(int id)
        {
            throw new NotImplementedException();
        }
        public NaesbEventMonitor GetByIdentity(string pipeline, string utility, string eventType, string cycle)
        {
            var obj = base.Get(p =>
                    p.Pipeline == pipeline &&
                    p.Utility == utility &&
                    p.EventType == eventType &&
                    p.Cycle == cycle
                )
                .Select(new ModelFactory().Map)
                .SingleOrDefault();

            return obj;
        }
        public List<NaesbEventMonitor> GetEventMonitors(DateTime runtimeDateTime)
        {
            var objs = base.Get(p =>
                    (p.ActiveStart <= runtimeDateTime &&
                    p.ActiveEnd >= runtimeDateTime) &&
                    runtimeDateTime.TimeOfDay > p.EventMonitorTime && p.LastCheckedTime == null ||
                    runtimeDateTime.TimeOfDay > p.EventMonitorTime && DbFunctions.DiffDays(p.LastCheckedTime, runtimeDateTime.Date) > 0  // DbFunctions.DiffDays -> date1 > date2 the result will be < 0 and date1 < date2 the result will be > 0
                )
                .Select(new ModelFactory().Map)
                .ToList();

            return objs;
        }
        //public List<NaesbEventMonitor> GetEventMonitors(DateTime runtimeDateTime)
        //{
        //    var objs = base.Get(p =>
        //                p.Id == 1000000039
        //        )
        //        .Select(new ModelFactory().Map)
        //        .ToList();

        //    return objs;
        //}
        public void Update(int id, params object[] list)
        {
            var entity = base.Get(id);

            foreach (object kvp in list)
            {
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Pipeline")
                {
                    entity.Pipeline = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Utility")
                {
                    entity.Utility = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "EventType")
                {
                    entity.EventType = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Cycle")
                {
                    entity.Cycle = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, short> && ((KeyValuePair<string, short>)kvp).Key == "SortSeq")
                {
                    entity.SortSeq = ((KeyValuePair<string, short>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "EventTypeDescription")
                {
                    entity.EventTypeDescription = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "CycleDescription")
                {
                    entity.CycleDescription = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, TimeSpan> && ((KeyValuePair<string, TimeSpan>)kvp).Key == "EventMonitorTime")
                {
                    entity.EventMonitorTime = ((KeyValuePair<string, TimeSpan>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, DateTime?> && ((KeyValuePair<string, DateTime?>)kvp).Key == "LastCheckedTime")
                {
                    entity.LastCheckedTime = ((KeyValuePair<string, DateTime?>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, DateTime> && ((KeyValuePair<string, DateTime>)kvp).Key == "ActiveStart")
                {
                    entity.ActiveStart = ((KeyValuePair<string, DateTime>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, DateTime> && ((KeyValuePair<string, DateTime>)kvp).Key == "ActiveEnd")
                {
                    entity.ActiveEnd = ((KeyValuePair<string, DateTime>)kvp).Value;
                }
            }

            base.Change(entity);
        }
    }
}
