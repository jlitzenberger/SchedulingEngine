using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.ScheduledQuantities.Naesb;

namespace Nomination.BusinessLayer.Services.ScheduledQuantity.Naesb
{
    public interface INaesbOperatorScheduledQuantitiesGet
    {
        List<NaesbScheduledQuantities> Invoke(DateTime gasday, string pipeline, string utility, string cycle);
        NaesbScheduledQuantities Invoke(int id);
    }
}
