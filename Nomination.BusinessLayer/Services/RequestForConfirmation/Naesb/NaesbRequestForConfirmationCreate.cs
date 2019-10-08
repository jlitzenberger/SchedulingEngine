using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CS.Common.Utilities;
using Nomination.Domain.Naesb;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.BusinessLayer.Services.RequestForConfirmation.Event;
using Nomination.Domain.RequestForConfirmation.Naesb;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Naesb
{
    public class NaesbRequestForConfirmationCreate : INaesbRequestForConfirmationCreate
    {
        private readonly ServiceSettings _settings;
        private readonly INaesbEventProcessUpdate _naesbEventProcessUpdateService;
        private readonly INaesbEventProcessCreate _naesbEventProcessCreateService;
        private readonly IRequestForConfirmationGet _requestForConfirmationGetService;
        private readonly IRequestForConfirmationEventCreate _requestForConfirmationEventCreateService;
        private readonly INaesbRequestForConfirmationHeaderGet _naesbRequestForConfirmationHeaderGetService;
        private readonly INaesbRequestForConfirmationGet _naesbRequestForConfirmationGetService;
        private readonly INaesbEventGet _naesbEventGetService;

        public NaesbRequestForConfirmationCreate(
              ServiceSettings settings
            , INaesbEventProcessUpdate naesbEventProcessUpdateService
            , INaesbEventProcessCreate naesbEventProcessCreateService
            , IRequestForConfirmationGet requestForConfirmationGetService
            , IRequestForConfirmationEventCreate requestForConfirmationEventCreateService
            , INaesbRequestForConfirmationHeaderGet naesbRequestForConfirmationHeaderGetService
            , INaesbRequestForConfirmationGet naesbRequestForConfirmationGetService
            , INaesbEventGet naesbEventGetService
            )
        {
            _settings = settings;
            _naesbEventProcessUpdateService = naesbEventProcessUpdateService;
            _naesbEventProcessCreateService = naesbEventProcessCreateService;
            _requestForConfirmationGetService = requestForConfirmationGetService;
            _requestForConfirmationEventCreateService = requestForConfirmationEventCreateService;
            _naesbRequestForConfirmationHeaderGetService = naesbRequestForConfirmationHeaderGetService;
            _naesbRequestForConfirmationGetService = naesbRequestForConfirmationGetService;
            _naesbEventGetService = naesbEventGetService;
        }
        
        public int Invoke(DateTime processStart, DateTime gasDay, string pipeline, string utility, string cycle)
        {
            ////get the domain RFC from the Repository
            //Domain.RequestForConfirmation.RequestForConfirmation rfc = _requestForConfirmationGetService.Invoke(pipeline, utility, gasDay, cycle);

            //if (rfc != null)
            //{
            //    XmlDocument domainXml = XmlTransformer.XmlSerialize(rfc, true);

            //    //create naesb event process
            //    int eventProcessId = _naesbEventProcessCreateService.Invoke(
            //        new NaesbEventProcess
            //        {
            //            Type = "RFC",
            //            GasDay = rfc.GasDay,
            //            Cycle = rfc.Cycle,
            //            //Pipeline = _pipelineService.Invoke(rfc.PartyIndentificaton.PipelineEntityId)?.Pipeline,
            //            //Utility = _utilityService.Invoke(rfc.PartyIndentificaton.UtilityEntityId)?.Utility,
            //            Pipeline = rfc.PartyIndentificaton.PipelineEntityId,
            //            Utility = rfc.PartyIndentificaton.UtilityEntityId,
            //            ProcessStart = processStart,
            //            DomainData = domainXml.InnerXml,
            //            UserId = _settings.UserId
            //        }
            //    );

            //    //save the domain RFC to the Repository
            //    _requestForConfirmationCreateService.Invoke(rfc);

            //    return eventProcessId;
            //}

            return 0;
        }

        public int Invoke(DateTime processStart, string fileName, Domain.RequestForConfirmation.RequestForConfirmation obj)
        {
            //map domain rfc model to the Naesb rfc model.
            NaesbRequestForConfirmation nrfc = _naesbRequestForConfirmationGetService.Invoke(obj);

            //serialize to xml
            XmlDocument domainXml = XmlTransformer.XmlSerialize(obj, true);
            //serialize Naesb NaesbRequestForConfirmation
            XmlDocument naesbXml = XmlTransformer.XmlSerialize(nrfc, true);

            //create naesb event process
            int eventProcessId = _naesbEventProcessCreateService.Invoke(
                new NaesbEventProcess
                {
                    Type = "RFC",
                    GasDay = obj.GasDay,
                    Cycle = obj.Cycle,
                    Pipeline = obj.PartyIndentificaton.PipelineEntity,
                    Utility = obj.PartyIndentificaton.UtilityEntity,
                    ProcessStart = processStart,
                    EdiFileName = fileName,
                    EdiData = naesbXml.InnerXml,
                    DomainData = domainXml.InnerXml,
                    UserId = _settings.UserId
                }
            );

            //get the naesb event
            var naesbEvent = _naesbEventGetService.Invoke("RFC", obj.PartyIndentificaton.PipelineEntity, obj.PartyIndentificaton.UtilityEntity, obj.Cycle);
            if (naesbEvent != null && naesbEvent.On == true) //if null then the pipeline/utility/cycle doesn't exist in Pegasys
            {
                //save the domain rfc to the repository
                _requestForConfirmationEventCreateService.Invoke(obj);

                //TODO: maybe make this use its own class instead of generic -> NaesbEventProcessCompletion
                //update ProcessEnd timestamp
                _naesbEventProcessUpdateService.Invoke(eventProcessId, new KeyValuePair<string, DateTime>("ProcessEnd", DateTime.Now));

                return eventProcessId;
            }

            throw new NaesbError
            {
                ReasonCode = "101",
                Value = "Pipeline/Utility/Cycle naesb event not found."
            };
        }
    }
}
