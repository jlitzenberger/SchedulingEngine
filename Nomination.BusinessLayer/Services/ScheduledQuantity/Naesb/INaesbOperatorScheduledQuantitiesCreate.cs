using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.ScheduledQuantities.Naesb;

namespace Nomination.BusinessLayer.Services.ScheduledQuantity.Naesb
{
    public interface INaesbOperatorScheduledQuantitiesCreate
    {
        int Invoke(DateTime processStart, NaesbScheduledQuantities naesbObj);
        int Invoke(DateTime processStart, FileInfo file);
    }
}
