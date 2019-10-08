using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.Domain.ScheduledQuantities.Naesb;

namespace Nomination.BusinessLayer.Services.ScheduledQuantity.Naesb
{
    public class NaesbOperatorScheduledQuantitiesGet : INaesbOperatorScheduledQuantitiesGet
    {
        private readonly INaesbEventProcessGet _naesbEventProcessGetService;

        public NaesbOperatorScheduledQuantitiesGet(
              INaesbEventProcessGet naesbEventProcessGetService)
        {
            _naesbEventProcessGetService = naesbEventProcessGetService;
        }
        public List<NaesbScheduledQuantities> Invoke(DateTime gasday, string pipeline, string utility, string cycle)
        {
            var nosqs = new List<NaesbScheduledQuantities>();

            var osqs = _naesbEventProcessGetService
                .Invoke(
                    "OSQ"
                    , gasday
                    , pipeline
                    , utility
                    , cycle);

            foreach (var osq in osqs)
            {
                var obj = CS.Common.Utilities.XmlTransformer.XmlDeserialize<NaesbScheduledQuantities>(osq.EdiData);

                nosqs.Add(obj);
            }

            return nosqs;
        }
        public NaesbScheduledQuantities Invoke(int id)
        {
            var nep = _naesbEventProcessGetService.Invoke(id);

            var osq = CS.Common.Utilities.XmlTransformer.XmlDeserialize<NaesbScheduledQuantities>(nep.EdiData);

            return osq;
        }
    }
}
