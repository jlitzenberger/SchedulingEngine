using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.Domain.RequestForConfirmation.Naesb;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Naesb
{
    public class NaesbRequestForConfirmationGet : INaesbRequestForConfirmationGet
    {
        private readonly ServiceSettings _settings;
        private readonly INaesbRepository _repository;
        private readonly INaesbEventProcessGet _naesbEventProcessGetService;
        private readonly INaesbRequestForConfirmationHeaderGet _naesbRequestForConfirmationHeaderGetService;
        private readonly INaesbPipelineGetByPipeline _naesbPipelineGetByPipelineService;
        private readonly INaesbUtilityGetByUtility _naesbUtilityGetByUtilityService;

        public NaesbRequestForConfirmationGet(
              ServiceSettings settings
            , INaesbRepository repository
            , INaesbEventProcessGet naesbEventProcessGetService
            , INaesbRequestForConfirmationHeaderGet naesbRequestForConfirmationHeaderGetService
            , INaesbPipelineGetByPipeline naesbPipelineGetByPipelineService
            , INaesbUtilityGetByUtility naesbUtilityGetByUtilityService)
        {
            _settings = settings;
            _repository = repository;
            _naesbEventProcessGetService = naesbEventProcessGetService;
            _naesbRequestForConfirmationHeaderGetService = naesbRequestForConfirmationHeaderGetService;
            _naesbPipelineGetByPipelineService = naesbPipelineGetByPipelineService;
            _naesbUtilityGetByUtilityService = naesbUtilityGetByUtilityService;
        }

        public NaesbRequestForConfirmation Invoke(Domain.RequestForConfirmation.RequestForConfirmation obj)
        {
            if (obj != null)
            {
                var header = _naesbRequestForConfirmationHeaderGetService.Invoke(obj);
                if (_settings.Environment == "prd")
                {
                    header.EnvironmentFlag = "P";
                }                

                NaesbRequestForConfirmation nrfc = new Common.ModelFactory().Map(header, obj);

                nrfc.PartyIndentificaton.ConfirmingPartyDuns = _naesbPipelineGetByPipelineService.Invoke(nrfc.PartyIndentificaton.ConfirmingPartyDuns)?.PipelineEntityId;
                nrfc.PartyIndentificaton.UtilityDunsNumber = _naesbUtilityGetByUtilityService.Invoke(nrfc.PartyIndentificaton.UtilityDunsNumber)?.UtilityEntityId;

                return nrfc;
            }

            return null;
        }

        public List<NaesbRequestForConfirmation> Invoke(DateTime gasday, string pipeline, string utility, string cycle)
        {
            var list = new List<NaesbRequestForConfirmation>();

            var objs = _naesbEventProcessGetService
                .Invoke(
                    "RFC"
                    , gasday
                    , pipeline
                    , utility
                    , cycle);

            foreach (var obj in objs)
            {
                list.Add(CS.Common.Utilities.XmlTransformer.XmlDeserialize<NaesbRequestForConfirmation>(obj.EdiData));
            }

            return list;
        }

        public NaesbRequestForConfirmation Invoke(int id)
        {
            var nep = _naesbEventProcessGetService.Invoke(id);

            var rfc = CS.Common.Utilities.XmlTransformer.XmlDeserialize<NaesbRequestForConfirmation>(nep.EdiData);

            return rfc;
        }
    }
}
