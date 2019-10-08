using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Interfaces
{
    public interface INaesbEventRepository
    {
        List<NaesbEvent> Find(Expression<Func<NaesbEvent, bool>> filter = null);
        //List<NaesbEvent> GetByPipeline(string eventType = null, string pipeline = null, string cycle = null, string utility = null);
        List<NaesbEvent> GetCyclesToProcess(DateTime date, string fileType);
        NaesbEvent GetByIdentity(string fileType, string pipeline, string utility, string cycle);
        NaesbEvent Get(int id);
        void Change(int id, NaesbEvent obj);
        void Update(int id, params object[] list);
    }
}
