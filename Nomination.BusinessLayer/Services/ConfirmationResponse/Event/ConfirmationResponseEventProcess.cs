using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;

namespace Nomination.BusinessLayer.Services.ConfirmationResponse.Event
{
    public class ConfirmationResponseEventProcess : IConfirmationResponseEventProcess
    {
        private readonly IConfirmationResponseEventRepository _repository;

        public ConfirmationResponseEventProcess(IConfirmationResponseEventRepository repository)
        {
            _repository = repository;
        }
        public void Invoke(string pipeline, string utility, DateTime gasDay, string cycle)
        {
            _repository.Process(pipeline, utility, gasDay, cycle);
        }
    }
}
