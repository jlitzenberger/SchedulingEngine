using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.Domain.ConfirmationResponse.Naesb;

namespace Nomination.BusinessLayer.Services.ConfirmationResponse.Naesb
{
    public class NaesbConfirmationResponseGet : INaesbConfirmationResponseGet
    {
        private readonly INaesbEventProcessRepository _naesbEventProcessRepository;
        private readonly INaesbEventProcessGet _naesbEventProcessGetService;
        public NaesbConfirmationResponseGet(
              INaesbEventProcessRepository naesbEventProcessRepository
            , INaesbEventProcessGet naesbEventProcessGetService)
        {
            _naesbEventProcessRepository = naesbEventProcessRepository;
            _naesbEventProcessGetService = naesbEventProcessGetService;
        }

        public List<NaesbConfirmationResponse> Invoke(DateTime gasday, string pipeline, string utility, string cycle)
        {
            var ncrs = new List<NaesbConfirmationResponse>();

            var crs = _naesbEventProcessGetService
                .Invoke(
                    "CR"
                    , gasday
                    , pipeline
                    , utility
                    , cycle);

            foreach (var cr in crs)
            {
                var obj = CS.Common.Utilities.XmlTransformer.XmlDeserialize<NaesbConfirmationResponse>(cr.EdiData);

                ncrs.Add(obj);
            }

            return ncrs;
        }
        public NaesbConfirmationResponse Invoke(int id)
        {
            var nep = _naesbEventProcessGetService.Invoke(id);

            var cr = CS.Common.Utilities.XmlTransformer.XmlDeserialize<NaesbConfirmationResponse>(nep.EdiData);

            return cr;
        }
    }
}
