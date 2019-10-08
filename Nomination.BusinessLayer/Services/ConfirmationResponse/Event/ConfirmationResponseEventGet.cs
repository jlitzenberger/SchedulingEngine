using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Nomination.Domain.ConfirmationResponse;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.Domain.ConfirmationResponse.Naesb;

namespace Nomination.BusinessLayer.Services.ConfirmationResponse.Event
{
    public class ConfirmationResponseEventGet : IConfirmationResponseEventGet
    {
        private readonly IConfirmationResponseEventRepository _repository;
        private readonly INaesbPipelineGet _naesbPipelineGetService;
        private readonly INaesbUtilityGet _naesbUtilityGetService;

        public ConfirmationResponseEventGet(
              IConfirmationResponseEventRepository repository
            , INaesbPipelineGet naesbPipelineGetService
            , INaesbUtilityGet naesbUtilityGetService)
        {
            _repository = repository;
            _naesbPipelineGetService = naesbPipelineGetService;
            _naesbUtilityGetService = naesbUtilityGetService;
        }

        public List<Domain.ConfirmationResponse.ConfirmationResponse> Invoke(string pipeline, string utility, DateTime gasday, string cycle)
        {
            return _repository.Get(pipeline, utility, gasday, cycle).ToList();
        }
        public Domain.ConfirmationResponse.ConfirmationResponse Invoke(int id)
        {
            return _repository.Get(id);
        }
        public Domain.ConfirmationResponse.ConfirmationResponse Invoke(NaesbConfirmationResponse obj)
        {
            //map NaesbConfirmationResponse to ConfirmationResponse
            return Map(CheckMissingCycle(new Nomination.BusinessLayer.Common.ModelFactory().Map(obj)));
        }
        public Domain.ConfirmationResponse.ConfirmationResponse Invoke(FileInfo file)
        {
            //deserialize GENTRAN EDI translated .xml file into Naesb EDI XmlDocument
            XmlDocument ediXml = CS.Common.Utilities.XmlTransformer.ConvertToXmlDocument(file, true);
            //deserialize XmlDocument into NaesbConfirmationResponse naesbObj
            NaesbConfirmationResponse naesbObj = CS.Common.Utilities.XmlTransformer.XmlDeserialize<NaesbConfirmationResponse>(ediXml.InnerXml);
            //map NaesbConfirmationResponse to ConfirmationResponse
            Domain.ConfirmationResponse.ConfirmationResponse cr = Map(CheckMissingCycle(new Common.ModelFactory().Map(naesbObj)));

            return cr;
        }
        private Nomination.Domain.ConfirmationResponse.ConfirmationResponse Map(Nomination.Domain.ConfirmationResponse.ConfirmationResponse obj)
        {
            if (obj != null)
            {
                obj.PartyIndentificaton.PipelineEntity = _naesbPipelineGetService.Invoke(obj.PartyIndentificaton.PipelineEntity).Pipeline;
                obj.PartyIndentificaton.UtilityEntity = _naesbUtilityGetService.Invoke(obj.PartyIndentificaton.UtilityEntity).Utility;

                return obj;
            }

            return null;
        }
        private Nomination.Domain.ConfirmationResponse.ConfirmationResponse CheckMissingCycle(Nomination.Domain.ConfirmationResponse.ConfirmationResponse obj)
        {
            //get the cycles for ANR because they don't provide them
            if (obj.PartyIndentificaton.PipelineEntity == "006958581")
            {
                string trackingId = obj.Locations[0]?.ContractNominations[0]?.Nominations[0]?.Id;

                //get naesb transaction master to get the Cycle
                var trans = _repository.Get("ANR", "RFC", trackingId);
                if (trans != null)
                {
                    obj.Cycle = trans.Cycle;
                }
                else
                {
                    throw new Exception("The ANR confirmation tracking id: " + trackingId + " is missing from the repository or the CR file.");
                }
            }

            return obj;
        }
    }
}
