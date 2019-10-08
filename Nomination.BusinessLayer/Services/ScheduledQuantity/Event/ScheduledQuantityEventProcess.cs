using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;

namespace Nomination.BusinessLayer.Services.ScheduledQuantity.Event
{
    public class ScheduledQuantityEventProcess : IScheduledQuantityEventProcess
    {
        private readonly IScheduledQuantityEventRepository _repository;

        public ScheduledQuantityEventProcess(IScheduledQuantityEventRepository repository)
        {
            _repository = repository;
        }
        public void Invoke(string pipeline, string utility, DateTime gasDay, string cycle)
        {
            _repository.Process(pipeline, utility, gasDay, cycle);
        }
    }
}
