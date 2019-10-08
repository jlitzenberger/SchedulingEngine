namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Event
{
    public interface IRequestForConfirmationEventChange
    {
        void Invoke(Nomination.Domain.RequestForConfirmation.RequestForConfirmation obj);
    }
}
