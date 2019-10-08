using System;
using System.Collections.Generic;
using System.Linq;
using Nomination.BusinessLayer.Interfaces;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Event
{
    public class RequestForConfirmationEventGet : IRequestForConfirmationEventGet
    {
        private readonly IRequestForConfirmationEventRepository _repository;

        public RequestForConfirmationEventGet(
              IRequestForConfirmationEventRepository repository)
        {
            _repository = repository;
        }

        public List<Domain.RequestForConfirmation.RequestForConfirmation> Invoke(string pipeline, string utility, DateTime gasday, string cycle)
        {
            return _repository.Get(pipeline, utility, gasday, cycle).ToList();
        }
        public Domain.RequestForConfirmation.RequestForConfirmation Invoke(int id)
        {
            return _repository.Get(id);
        }
    }
}
