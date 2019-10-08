using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services.Naesb;

namespace Nomination.BusinessLayer.Services.ConfirmationResponse.Event
{
    public class ConfirmationResponseEventCreate : IConfirmationResponseEventCreate
    {
        private readonly ServiceSettings _settings;
        private readonly IConfirmationResponseEventRepository _confirmationResponseEventRepositoryService;
        private readonly IConfirmationResponseEventProcess _confirmationResponseProcessService;
        private readonly INaesbEventGet _naesbEventGetService;

        public ConfirmationResponseEventCreate(
              ServiceSettings settings
            , IConfirmationResponseEventRepository confirmationResponseEventRepositoryService
            , IConfirmationResponseEventProcess confirmationResponseProcessService
            , INaesbEventGet naesbEventGetService)
        {
            _settings = settings;
            _confirmationResponseEventRepositoryService = confirmationResponseEventRepositoryService;
            _confirmationResponseProcessService = confirmationResponseProcessService;
            _naesbEventGetService = naesbEventGetService;
        }

        public int Invoke(Nomination.Domain.ConfirmationResponse.ConfirmationResponse obj)
        {
            //create the naesb transaction
            int id = _confirmationResponseEventRepositoryService.Create(obj, _settings.UserId);

            //get the naesb event
            var naesbEvent = _naesbEventGetService.Invoke("CR", obj.PartyIndentificaton.PipelineEntity, obj.PartyIndentificaton.UtilityEntity, obj.Cycle);

            //process the ConfirmationResponse
            _confirmationResponseProcessService.Invoke(naesbEvent.Pipeline, naesbEvent.Utility, obj.GasDay, naesbEvent.Cycle);
            
            return id;
        }
    }
}