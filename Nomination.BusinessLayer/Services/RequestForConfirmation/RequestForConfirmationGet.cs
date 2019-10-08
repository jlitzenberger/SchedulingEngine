using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services.Naesb;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation
{
    public class RequestForConfirmationGet : IRequestForConfirmationGet
    {
        private readonly IRequestForConfirmationRepository _repository;
        private readonly INaesbPipelineGet _naesbPipelineGetService;
        private readonly INaesbUtilityGet _naesbUtilityGetService;

        public RequestForConfirmationGet(
              IRequestForConfirmationRepository repository
            , INaesbPipelineGet naesbPipelineGetService
            , INaesbUtilityGet naesbUtilityGetService)
        {
            _repository = repository;
            _naesbPipelineGetService = naesbPipelineGetService;
            _naesbUtilityGetService = naesbUtilityGetService;
        }

        public Nomination.Domain.RequestForConfirmation.RequestForConfirmation Invoke(string pipeline, string utility, DateTime gasDay, string cycle)
        {
            return Map(_repository.Get(pipeline, utility, gasDay, cycle));
        }
        //public Nomination.Domain.RequestForConfirmation.RequestForConfirmation Invoke(int id)
        //{
        //    return Map(_repository.Get(id));
        //}
        private Nomination.Domain.RequestForConfirmation.RequestForConfirmation Map(Nomination.Domain.RequestForConfirmation.RequestForConfirmation obj)
        {
            if (obj != null)
            {
                obj.PartyIndentificaton.PipelineEntity = _naesbPipelineGetService.Invoke(obj.PartyIndentificaton.PipelineEntity).Pipeline;
                obj.PartyIndentificaton.UtilityEntity = _naesbUtilityGetService.Invoke(obj.PartyIndentificaton.UtilityEntity).Utility;

                return obj;
            }

            return null;
        }
    }
}
