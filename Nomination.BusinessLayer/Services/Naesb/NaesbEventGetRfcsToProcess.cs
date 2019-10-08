using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services.RequestForConfirmation;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventGetRfcsToProcess : INaesbEventGetRfcsToProcess
    {
        private readonly INaesbEventRepository _repository;
        private readonly IRequestForConfirmationGet _requestForConfirmationGetService;

        public NaesbEventGetRfcsToProcess(
              INaesbEventRepository repository
            , IRequestForConfirmationGet requestForConfirmationGetService)
        {
            _repository = repository;
            _requestForConfirmationGetService = requestForConfirmationGetService;
        }
        public List<Domain.RequestForConfirmation.RequestForConfirmation> Invoke(DateTime date)
        {
            List<Domain.RequestForConfirmation.RequestForConfirmation> rfcs = new List<Domain.RequestForConfirmation.RequestForConfirmation>();
            //return all potential valid rfcs to be processed
            List<NaesbEvent> naesbEvents = _repository.GetCyclesToProcess(date, "RFC");

            foreach (var naesbEvent in naesbEvents)
            {
                //this will always be current because if an RFC for a cycle doesn't go out...then it never does for that cycle.
                DateTime gasDay = date.AddDays(naesbEvent.OffSet != null ? Convert.ToDouble(naesbEvent.OffSet) : 0);
                //get the domain RFC from the repository
                var rfc = GetRequestForConfirmation(naesbEvent.Pipeline, naesbEvent.Utility, gasDay, naesbEvent.Cycle);

                if (rfc != null)
                {
                    rfcs.Add(rfc);
                }
            }

            return rfcs;
        }
        private Domain.RequestForConfirmation.RequestForConfirmation GetRequestForConfirmation(string pipeline, string utility, DateTime gasDay, string cycle)
        {
            Domain.RequestForConfirmation.RequestForConfirmation obj = _requestForConfirmationGetService.Invoke(pipeline, utility, gasDay, cycle);

            return obj;
        }
    }
}
