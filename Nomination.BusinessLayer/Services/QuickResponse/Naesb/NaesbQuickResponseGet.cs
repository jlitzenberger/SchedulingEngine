using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Services.ConfirmationResponse.Event;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.Domain.QuickResponse.Naesb;

namespace Nomination.BusinessLayer.Services.QuickResponse.Naesb
{
    public class NaesbQuickResponseGet : INaesbQuickResponseGet
    {
        private readonly IConfirmationResponseEventGet _confirmationResponseEventGetService;
        private readonly INaesbQuickResponseHeaderGet _naesbQuickResponseHeaderGetService;
        private readonly INaesbPipelineGetByPipeline _naesbPipelineGetByPipelineService;
        private readonly INaesbUtilityGetByUtility _naesbUtilityGetByUtilityService;

        public NaesbQuickResponseGet(
                IConfirmationResponseEventGet confirmationResponseEventGetService
              , INaesbQuickResponseHeaderGet naesbQuickResponseHeaderGetService
              , INaesbPipelineGetByPipeline naesbPipelineGetByPipelineService
              , INaesbUtilityGetByUtility naesbUtilityGetByUtilityService)
        {
            _confirmationResponseEventGetService = confirmationResponseEventGetService;
            _naesbQuickResponseHeaderGetService = naesbQuickResponseHeaderGetService;
            _naesbPipelineGetByPipelineService = naesbPipelineGetByPipelineService;
            _naesbUtilityGetByUtilityService = naesbUtilityGetByUtilityService;
        }
        public NaesbQuickResponse Invoke(Domain.ConfirmationResponse.ConfirmationResponse confirmationResponse)
        {
            NaesbQuickResponse nqr = new NaesbQuickResponse();

            nqr.Header = _naesbQuickResponseHeaderGetService.Invoke(confirmationResponse);
            nqr.PurchaseOrderNumber = confirmationResponse.PurchaseOrderNumber;
            nqr.PurposeCode = "27";
            nqr.PartyIndentificaton = new PartyIndentificaton();
            nqr.PartyIndentificaton.ConfirmingPartyDuns = _naesbPipelineGetByPipelineService.Invoke(confirmationResponse.PartyIndentificaton.PipelineEntity)?.PipelineEntityId;
            nqr.PartyIndentificaton.UtilityDunsNumber = _naesbUtilityGetByUtilityService.Invoke(confirmationResponse.PartyIndentificaton.UtilityEntity)?.UtilityEntityId;

            return nqr;
        }
    }
}
