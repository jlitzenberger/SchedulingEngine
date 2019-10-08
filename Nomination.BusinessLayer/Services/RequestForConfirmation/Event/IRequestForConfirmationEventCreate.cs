namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Event
{
    public interface IRequestForConfirmationEventCreate
    {
        int Invoke(Nomination.Domain.RequestForConfirmation.RequestForConfirmation obj);
    }
}
