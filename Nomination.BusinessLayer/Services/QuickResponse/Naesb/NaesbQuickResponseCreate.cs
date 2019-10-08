using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CS.Common.Utilities;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.Domain.Naesb;
using Nomination.Domain.QuickResponse.Naesb;

namespace Nomination.BusinessLayer.Services.QuickResponse.Naesb
{
    public class NaesbQuickResponseCreate : INaesbQuickResponseCreate
    {
        private readonly ServiceSettings _settings;
        private readonly INaesbQuickResponseGet _naesbQuickResponseGetService;
        private readonly INaesbEventProcessUpdate _naesbEventProcessUpdateService;
        private readonly INaesbEventProcessCreate _naesbEventProcessCreateService;
        //private readonly INaesbPipelineGet _naesbPipelineGetService;
        //private readonly INaesbUtilityGet _naesbUtilityGetService;

        public NaesbQuickResponseCreate(
              ServiceSettings settings
            , INaesbQuickResponseGet naesbQuickResponseGetService
            , INaesbEventProcessUpdate naesbEventProcessUpdateService
            , INaesbEventProcessCreate naesbEventProcessCreateService
            //, INaesbPipelineGet naesbPipelineGetService
            //, INaesbUtilityGet naesbUtilityGetService
            )
        {
            _settings = settings;
            _naesbQuickResponseGetService = naesbQuickResponseGetService;
            _naesbEventProcessUpdateService = naesbEventProcessUpdateService;
            _naesbEventProcessCreateService = naesbEventProcessCreateService;
            //_naesbPipelineGetService = naesbPipelineGetService;
            //_naesbUtilityGetService = naesbUtilityGetService;
        }

        //public int Invoke(DateTime processStart, DateTime gasDay, string cycle, string fileName, NaesbQuickResponse nqr)
        //{
            ////serialize Naesb NaesbRequestForConfirmation
            //XmlDocument naesbXml = XmlTransformer.XmlSerialize(nqr, true);

            ////create naesb event process
            //int eventProcessId = _naesbEventProcessCreateService.Invoke(
            //    new NaesbEventProcess
            //    {
            //        Type = "QR",
            //        GasDay = gasDay,
            //        Cycle = cycle,
            //        Pipeline = _naesbPipelineGetService.Invoke(nqr.PartyIndentificaton.ConfirmingPartyDuns).Pipeline,
            //        Utility = _naesbUtilityGetService.Invoke(nqr.PartyIndentificaton.UtilityDunsNumber).Utility,
            //        ProcessStart = processStart,
            //        EdiFileName = fileName,
            //        EdiData = naesbXml.InnerXml,
            //        DomainData = null,
            //        UserId = _settings.UserId
            //    }
            //);

            ////TODO: maybe make this use its own class instead of generic -> NaesbEventProcessCompletion
            ////update ProcessEnd timestamp
            //_naesbEventProcessUpdateService.Invoke(eventProcessId, new KeyValuePair<string, DateTime>("ProcessEnd", DateTime.Now));

            //return eventProcessId;
        //}

        public int Invoke(DateTime processStart, string fileName, Domain.ConfirmationResponse.ConfirmationResponse cr)
        {
            //transform ConfirmationResponse
            NaesbQuickResponse nqr = _naesbQuickResponseGetService.Invoke(cr);

            //serialize Naesb NaesbRequestForConfirmation
            XmlDocument naesbXml = XmlTransformer.XmlSerialize(nqr, true);

            //create naesb event process
            int eventProcessId = _naesbEventProcessCreateService.Invoke(
                new NaesbEventProcess
                {
                    Type = "QR",
                    GasDay = cr.GasDay,
                    Cycle = cr.Cycle,
                    Pipeline = cr.PartyIndentificaton.PipelineEntity,
                    Utility = cr.PartyIndentificaton.UtilityEntity,
                    ProcessStart = processStart,
                    EdiFileName = fileName,
                    EdiData = naesbXml.InnerXml,
                    DomainData = null,  // there is no business QuickResponse
                    UserId = _settings.UserId
                }
            );

            //TODO: maybe make this use its own class instead of generic -> NaesbEventProcessCompletion
            //update ProcessEnd timestamp
            _naesbEventProcessUpdateService.Invoke(eventProcessId, new KeyValuePair<string, DateTime>("ProcessEnd", DateTime.Now));

            return eventProcessId;
        }
    }
}
