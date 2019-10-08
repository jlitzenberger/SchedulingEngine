using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public interface INaesbEventGetList
    {
        List<Nomination.Domain.Naesb.NaesbEvent> Invoke(DateTime date, string fileType);
        List<Nomination.Domain.Naesb.NaesbEvent> Invoke(string eventType = null, string pipeline = null, string cycle = null, string utility = null);
    }
}
