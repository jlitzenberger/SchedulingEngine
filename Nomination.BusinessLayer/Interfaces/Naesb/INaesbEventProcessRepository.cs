using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Interfaces
{
    public interface INaesbEventProcessRepository
    {
        NaesbEventProcess Get(int id);
        List<NaesbEventProcess> GetByIdentity(string fileType, DateTime gasday, string pipeline, string utility, string cycle);
        int Create(NaesbEventProcess obj);
        void Change(int id, NaesbEventProcess obj);
        //void Update(int id, NaesbEventProcess obj);
        void Update(int id, params object[] list);
        //void UpdateEventProcessRfcCompletion(int id, DateTime processEnd, string ediFileName, string ediData, string domainData, string userId);
    }
}
