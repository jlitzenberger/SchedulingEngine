using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Persistence.Shared;

namespace Nomination.Persistence.Naesb
{
    public class NaesbEventProcessRespository : Repository<tb_naesb_event_process>, INaesbEventProcessRepository
    {
        public NaesbEventProcess Get(int id)
        {
            return new Nomination.Persistence.Common.ModelFactory().Map(base.Get(id));
        }
        public List<NaesbEventProcess> GetByIdentity(string fileType, DateTime gasday, string pipeline, string utility, string cycle)
        {
            var obj = base.Get(p =>
                    p.FileType == fileType &&
                    p.GasDay == gasday &&
                    p.PipelineCd == pipeline &&
                    p.CompanyCd == utility &&
                    p.CycleCd == cycle
                )
                .Select(new Nomination.Persistence.Common.ModelFactory().Map)
                .ToList();

            return obj;
        }
        public int Create(NaesbEventProcess obj)
        {
            //map from domain obj to entity
            var entity = new Nomination.Persistence.Common.ModelFactory().Map(obj);

            base.Add(entity);

            return Convert.ToInt32(entity.EventProcessId);
        }
        public void Change(int id, NaesbEventProcess obj)
        {
            obj.Id = id;
            //map from domain obj to entity
            var entity = new Nomination.Persistence.Common.ModelFactory().Map(obj);

            base.Change(entity);
        }
        public void Update(int id, params object[] list)
        {
            var entity = base.Get(id);

            foreach (object kvp in list)
            {
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Type")
                {
                    entity.FileType = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, DateTime> && ((KeyValuePair<string, DateTime>)kvp).Key == "GasDay")
                {
                    entity.GasDay = ((KeyValuePair<string, DateTime>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Cycle")
                {
                    entity.CycleCd = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Pipeline")
                {
                    entity.PipelineCd = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Utility")
                {
                    entity.CompanyCd = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, DateTime> && ((KeyValuePair<string, DateTime>)kvp).Key == "ProcessStart")
                {
                    entity.ProcessStart = ((KeyValuePair<string, DateTime>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, DateTime> && ((KeyValuePair<string, DateTime>)kvp).Key == "ProcessEnd")
                {
                    entity.ProcessEnd = ((KeyValuePair<string, DateTime>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "EdiFileName")
                {
                    entity.EDIFileName = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "EdiData")
                {
                    entity.EDIData = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "DomainData")
                {
                    entity.BUSData = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "StackTrace")
                {
                    entity.StackTrace = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "UserId")
                {
                    entity.row_lst_upd_userid = ((KeyValuePair<string, string>)kvp).Value;
                }
            }

            base.Change(entity);
        }
    }
}
