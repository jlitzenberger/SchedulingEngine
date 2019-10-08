using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventUpdate : INaesbEventUpdate
    {
        private readonly INaesbEventRepository _repository;
        public NaesbEventUpdate(INaesbEventRepository repository)
        {
            _repository = repository;
        }
        public void Invoke(int id, DateTime? processedTime)
        {
            _repository.Update(id, new KeyValuePair<string, DateTime?>("ProcessedTime", processedTime));
        }

        public void Invoke(int id, NaesbEvent obj)
        {
            _repository.Update(id,
                new KeyValuePair<string, string>("Type", obj.FileType),
                new KeyValuePair<string, string>("Cycle", obj.Cycle),
                new KeyValuePair<string, string>("Pipeline", obj.Pipeline),
                new KeyValuePair<string, string>("Utility", obj.Utility),
                new KeyValuePair<string, DateTime?>("ProcessedTime", obj.ProcessedTime),
                new KeyValuePair<string, TimeSpan?>("CycleStart", obj.CycleStart),
                new KeyValuePair<string, TimeSpan?>("CycleEnd", obj.CycleEnd),
                new KeyValuePair<string, short?>("OffSet", obj.OffSet),
                new KeyValuePair<string, bool?>("On", obj.On)
            );
        }
    }
}
