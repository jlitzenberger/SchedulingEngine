using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.Domain.ScheduledQuantities;
using Nomination.Domain.ScheduledQuantities.Naesb;

namespace Nomination.BusinessLayer.Services.ScheduledQuantity.Event
{
    public class ScheduledQuantityEventGet : IScheduledQuantityEventGet
    {
        private readonly IScheduledQuantityEventRepository _repository;
        private readonly INaesbPipelineGet _naesbPiplineGetService;
        private readonly INaesbUtilityGet _naesbUtilityGetService;

        public ScheduledQuantityEventGet(
              IScheduledQuantityEventRepository repository
            , INaesbPipelineGet naesbPiplineGetService
            , INaesbUtilityGet naesbUtilityGetService)
        {
            _repository = repository;
            _naesbPiplineGetService = naesbPiplineGetService;
            _naesbUtilityGetService = naesbUtilityGetService;
        }

        public List<ScheduledQuantities> Invoke(string pipeline, string utility, DateTime gasday, string cycle)
        {
            return _repository.Get(pipeline, utility, gasday, cycle).ToList();
        }
        public ScheduledQuantities Invoke(int id)
        {
            return _repository.Get(id);
        }
        public ScheduledQuantities Invoke(NaesbScheduledQuantities obj)
        {
            //map NaesbScheduledQuantities to ScheduledQuantities
            return Map(new Nomination.BusinessLayer.Common.ModelFactory().Map(obj));
        }
        public ScheduledQuantities Invoke(FileInfo file)
        {
            //deserialize GENTRAN EDI translated .xml file into Naesb EDI XmlDocument
            XmlDocument ediXml = CS.Common.Utilities.XmlTransformer.ConvertToXmlDocument(file, true);
            //deserialize XmlDocument into NaesbScheduledQuantities naesbObj
            NaesbScheduledQuantities naesbObj = CS.Common.Utilities.XmlTransformer.XmlDeserialize<NaesbScheduledQuantities>(ediXml.InnerXml);
            //map NaesbScheduledQuantities to ScheduledQuantities
            ScheduledQuantities osq = Map(new Common.ModelFactory().Map(naesbObj));

            return osq;
        }
        private ScheduledQuantities Map(ScheduledQuantities obj)
        {
            if (obj != null)
            {
                obj.PartyIndentificaton.PipelineEntity = _naesbPiplineGetService.Invoke(obj.PartyIndentificaton.PipelineEntity).Pipeline;
                obj.PartyIndentificaton.UtilityEntity = _naesbUtilityGetService.Invoke(obj.PartyIndentificaton.UtilityEntity).Utility;

                //TODO: Location.ContractNominations.NomsContractInfo probably should not exist and be raised to a higher level
                if (obj.PartyIndentificaton.PipelineEntity == "NGPL" && (obj.PartyIndentificaton.UtilityEntity == "PGL" || obj.PartyIndentificaton.UtilityEntity == "NSG"))
                {
                    foreach (var location in obj.Locations)
                    {
                        foreach (var contractNomination in location.ContractNominations)
                        {
                            foreach (var nomination in contractNomination.Nominations)
                            {
                                //nomination.NomsContractInfo.ContractId = nomination?.Stream?.ContractId;
                                nomination.Stream.ContractId = nomination?.NomsContractInfo?.ContractId;
                            }
                        }
                    }
                }

                return obj;
            }

            return null;
        }
    }
}
