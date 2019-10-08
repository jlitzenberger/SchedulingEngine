using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services.Naesb;

namespace Nomination.BusinessLayer.Services.ScheduledQuantity.Event
{
    public class ScheduledQuantityEventCreate : IScheduledQuantityEventCreate
    {
        private readonly ServiceSettings _settings;
        private readonly IScheduledQuantityEventRepository _scheduledQuantityService;
        private readonly IScheduledQuantityEventProcess _scheduledQuantityProcessService;
        private readonly INaesbEventGet _naesbEventGetService;

        public ScheduledQuantityEventCreate(
              ServiceSettings settings
            , IScheduledQuantityEventRepository scheduledQuantityService
            , IScheduledQuantityEventProcess scheduledQuantityProcessService
            , INaesbEventGet naesbEventGetService)
        {
            _settings = settings;
            _scheduledQuantityService = scheduledQuantityService;
            _scheduledQuantityProcessService = scheduledQuantityProcessService;
            _naesbEventGetService = naesbEventGetService;
        }

        public int Invoke(Nomination.Domain.ScheduledQuantities.ScheduledQuantities obj)
        {
            //create the naesb transaction
            int id = _scheduledQuantityService.Create(obj, _settings.UserId);

            //get the naesb event
            var naesbEvent = _naesbEventGetService.Invoke("OSQ", obj.PartyIndentificaton.PipelineEntity, obj.PartyIndentificaton.UtilityEntity, obj.Cycle);

            //process the ScheduledQuantity
            _scheduledQuantityProcessService.Invoke(naesbEvent.Pipeline, naesbEvent.Utility, obj.GasDay, naesbEvent.Cycle);

            return id;
        }
    }
}
