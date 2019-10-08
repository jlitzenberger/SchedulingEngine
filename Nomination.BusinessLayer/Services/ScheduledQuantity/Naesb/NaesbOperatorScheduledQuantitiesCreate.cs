using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.BusinessLayer.Services.ScheduledQuantity.Event;
using Nomination.Domain.Naesb;
using Nomination.Domain.ScheduledQuantities.Naesb;

namespace Nomination.BusinessLayer.Services.ScheduledQuantity.Naesb
{
    public class NaesbOperatorScheduledQuantitiesCreate : INaesbOperatorScheduledQuantitiesCreate
    {
        private readonly ServiceSettings _settings;
        private readonly INaesbEventProcessUpdate _naesbEventProcessUpdateService;
        private readonly INaesbEventProcessCreate _naesbEventProcessCreateService;
        private readonly IScheduledQuantityEventCreate _scheduledQuantityEventCreateService;
        private readonly IScheduledQuantityEventGet _scheduledQuantityEventGetService;
        private readonly INaesbEventGet _naesbEventGetService;
        public NaesbOperatorScheduledQuantitiesCreate(
              ServiceSettings settings
            , INaesbEventProcessUpdate naesbEventProcessUpdateService
            , INaesbEventProcessCreate naesbEventProcessCreateService
            , IScheduledQuantityEventCreate scheduledQuantityEventCreateService
            , IScheduledQuantityEventGet scheduledQuantityEventGetService
            , INaesbEventGet naesbEventGetService)
        {
            _settings = settings;
            _naesbEventProcessUpdateService = naesbEventProcessUpdateService;
            _naesbEventProcessCreateService = naesbEventProcessCreateService;
            _scheduledQuantityEventCreateService = scheduledQuantityEventCreateService;
            _scheduledQuantityEventGetService = scheduledQuantityEventGetService;
            _naesbEventGetService = naesbEventGetService;
        }

        public int Invoke(DateTime processStart, NaesbScheduledQuantities naesbObj)
        {
            //serialize into naesb XmlDocument
            XmlDocument naesbXml = CS.Common.Utilities.XmlTransformer.XmlSerialize(naesbObj, true);

            int id = CreateScheduledQuantities(processStart, null, naesbObj, naesbXml);

            return id;
        }
        public int Invoke(DateTime processStart, FileInfo file)
        {
            //serialize naesb translated .xml file into naesb XmlDocument
            XmlDocument naesbXml = CS.Common.Utilities.XmlTransformer.ConvertToXmlDocument(file, true);

            //deserialize XmlDocument into NaesbConfirmationResponse naesbObj
            NaesbScheduledQuantities naesbObj = CS.Common.Utilities.XmlTransformer.XmlDeserialize<NaesbScheduledQuantities>(naesbXml.InnerXml);

            int id = CreateScheduledQuantities(processStart, file.Name, naesbObj, naesbXml);

            return id;
        }
        private int CreateScheduledQuantities(DateTime processStart, string fileName, NaesbScheduledQuantities naesbObj, XmlDocument naesbXml)
        {
            //map NaesbScheduledQuantities to ScheduledQuantities
            Nomination.Domain.ScheduledQuantities.ScheduledQuantities osq = _scheduledQuantityEventGetService.Invoke(naesbObj);

            //serialize to xml
            XmlDocument domainXml = CS.Common.Utilities.XmlTransformer.XmlSerialize(osq, true);

            //instantiate naesb event process
            NaesbEventProcess obj = new NaesbEventProcess
            {
                Type = "OSQ",
                GasDay = osq.GasDay,
                Cycle = osq.Cycle,
                Pipeline = osq.PartyIndentificaton.PipelineEntity,
                Utility = osq.PartyIndentificaton.UtilityEntity,
                ProcessStart = processStart,
                EdiFileName = fileName,
                EdiData = naesbXml.InnerXml,
                DomainData = domainXml.InnerXml,
                UserId = _settings.UserId
            };

            //get the naesb event
            var naesbEvent = _naesbEventGetService.Invoke("OSQ", obj.Pipeline, obj.Utility, obj.Cycle);
            if (naesbEvent != null && naesbEvent.On == true) //if null then the pipeline/utility/cycle doesn't exist in Pegasys
            {
                //create naesb event process
                int eventProcessId = _naesbEventProcessCreateService.Invoke(obj);

                //create the ScheduledQuantities
                _scheduledQuantityEventCreateService.Invoke(osq);

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
