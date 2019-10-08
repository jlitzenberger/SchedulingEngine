using Nomination.BusinessLayer.Interfaces;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Event
{
    public class RequestForConfirmationEventCreate : IRequestForConfirmationEventCreate
    {
        private readonly ServiceSettings _settings;
        private readonly IRequestForConfirmationEventRepository _repository;
        
        public RequestForConfirmationEventCreate(ServiceSettings settings, IRequestForConfirmationEventRepository repository)
        {
            _settings = settings;
            _repository = repository;
        }
        public int Invoke(Nomination.Domain.RequestForConfirmation.RequestForConfirmation obj)
        {
            return _repository.Create(obj, _settings.UserId);
        }
    }
}
