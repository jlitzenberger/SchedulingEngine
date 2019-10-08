using System;
using Nomination.BusinessLayer.Interfaces;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Event
{
    public class RequestForConfirmationEventChange : IRequestForConfirmationEventChange
    {
        private readonly IRequestForConfirmationEventRepository _repository;

        public RequestForConfirmationEventChange(IRequestForConfirmationEventRepository repository)
        {
            _repository = repository;
        }
        public void Invoke(Domain.RequestForConfirmation.RequestForConfirmation obj)
        {
            throw new NotImplementedException();
        }
    }
}
