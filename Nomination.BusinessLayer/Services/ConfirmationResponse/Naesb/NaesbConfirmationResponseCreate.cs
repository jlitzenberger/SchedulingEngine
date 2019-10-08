using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Nomination.BusinessLayer.Services.ConfirmationResponse.Event;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.Domain.Naesb;
using Nomination.Domain.ConfirmationResponse.Naesb;

namespace Nomination.BusinessLayer.Services.ConfirmationResponse.Naesb
{
    public class NaesbConfirmationResponseCreate : INaesbConfirmationResponseCreate
    {
        private readonly ServiceSettings _settings;
        private readonly INaesbEventProcessUpdate _naesbEventProcessUpdateService;
        private readonly INaesbEventProcessCreate _naesbEventProcessCreateService;
        private readonly IConfirmationResponseEventCreate _confirmationResponseCreateService;
        private readonly IConfirmationResponseEventGet _confirmationResponseGetService;
        private readonly INaesbEventGet _naesbEventGetService;

        public NaesbConfirmationResponseCreate(
              ServiceSettings settings
            , INaesbEventProcessUpdate naesbEventProcessUpdateService
            , INaesbEventProcessCreate naesbEventProcessCreateService
            , IConfirmationResponseEventCreate confirmationResponseCreateService
            , IConfirmationResponseEventGet confirmationResponseGetService
            , INaesbEventGet naesbEventGetService
            )
        {
            _settings = settings;
            _naesbEventProcessUpdateService = naesbEventProcessUpdateService;
            _naesbEventProcessCreateService = naesbEventProcessCreateService;
            _confirmationResponseCreateService = confirmationResponseCreateService;
            _confirmationResponseGetService = confirmationResponseGetService;
            _naesbEventGetService = naesbEventGetService;
        }

        public int Invoke(DateTime processStart, NaesbConfirmationResponse naesbObj)
        {
            //serialize into naesb XmlDocument
            XmlDocument naesbXml = CS.Common.Utilities.XmlTransformer.XmlSerialize(naesbObj, true);

            int id = CreateConfirmationResponse(processStart, null, naesbObj, naesbXml);

            return id;
        }
        public int Invoke(DateTime processStart, FileInfo file)
        {
            //serialize naesb translated .xml file into naesb XmlDocument
            XmlDocument naesbXml = CS.Common.Utilities.XmlTransformer.ConvertToXmlDocument(file, true);

            //deserialize XmlDocument into NaesbConfirmationResponse naesbObj
            NaesbConfirmationResponse naesbObj = CS.Common.Utilities.XmlTransformer.XmlDeserialize<NaesbConfirmationResponse>(naesbXml.InnerXml);

            int id = CreateConfirmationResponse(processStart, file.Name, naesbObj, naesbXml);

            return id;
        }
        private int CreateConfirmationResponse(DateTime processStart, string fileName, NaesbConfirmationResponse naesbObj, XmlDocument naesbXml)
        {
            //map NaesbConfirmationResponse to ConfirmationResponse
            Nomination.Domain.ConfirmationResponse.ConfirmationResponse cr = _confirmationResponseGetService.Invoke(naesbObj);

            //serialize to xml
            XmlDocument domainXml = CS.Common.Utilities.XmlTransformer.XmlSerialize(cr, true);

            //instantiate naesb event process
            NaesbEventProcess obj = new NaesbEventProcess
            {
                Type = "CR",
                GasDay = cr.GasDay,
                Cycle = cr.Cycle,
                Pipeline = cr.PartyIndentificaton.PipelineEntity,
                Utility = cr.PartyIndentificaton.UtilityEntity,
                ProcessStart = processStart,
                EdiFileName = fileName,
                EdiData = naesbXml.InnerXml,
                DomainData = domainXml.InnerXml,
                UserId = _settings.UserId
            };

            //get the naesb event
            var naesbEvent = _naesbEventGetService.Invoke("CR", obj.Pipeline, obj.Utility, obj.Cycle);
            if (naesbEvent != null && naesbEvent.On == true) //if null then the pipeline/utility/cycle doesn't exist in Pegasys
            {
                //create naesb event process
                int eventProcessId = _naesbEventProcessCreateService.Invoke(obj);

                //create the ConfirmationResponse
                _confirmationResponseCreateService.Invoke(cr);

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





        //private NaesbEventProcess CreateConfirmationResponse(DateTime processStart, FileInfo file, XmlDocument ediXml)
        //{
        //    //deserialize XmlDocument into NaesbConfirmationResponse naesbObj
        //    NaesbConfirmationResponse naesbObj = CS.Common.Utilities.XmlTransformer.XmlDeserialize<NaesbConfirmationResponse>(ediXml.InnerXml);

        //    //map NaesbConfirmationResponse to ConfirmationResponse
        //    Nomination.Domain.ConfirmationResponse.ConfirmationResponse cr = new Nomination.BusinessLayer.Common.ModelFactory().Map(naesbObj);

        //    //serialize to xml
        //    XmlDocument domainXml = CS.Common.Utilities.XmlTransformer.XmlSerialize(cr, true);

        //    //create naesb event process
        //    NaesbEventProcess obj = new NaesbEventProcess
        //    {
        //        Type = "CR",
        //        GasDay = cr.GasDay,
        //        Cycle = cr.Cycle,
        //        Pipeline = _pipelineService.Invoke(cr.PartyIndentificaton.PipelineEntityId)?.Pipeline,
        //        Utility = _utilityService.Invoke(cr.PartyIndentificaton.UtilityEntityId)?.Utility,
        //        ProcessStart = processStart,
        //        EdiFileName = file.Name,
        //        EdiData = ediXml.InnerXml,
        //        DomainData = domainXml.InnerXml,
        //        UserId = _settings.UserId
        //    };

        //    return obj;
        //}
    }
}