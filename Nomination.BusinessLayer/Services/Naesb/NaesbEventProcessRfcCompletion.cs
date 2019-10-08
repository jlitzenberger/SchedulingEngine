using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventProcessRfcCompletion : INaesbEventProcessRfcCompletion
    {
        private readonly INaesbEventProcessRepository _repository;
        public NaesbEventProcessRfcCompletion(INaesbEventProcessRepository repository)
        {
            _repository = repository;
        }
        public void Invoke(int id, DateTime processEnd, string ediFileName, string ediData, string domainData, string userId)
        {
            _repository.Update(id,
                new KeyValuePair<string, DateTime>("ProcessEnd", processEnd),
                new KeyValuePair<string, string>("EdiFileName", ediFileName),
                new KeyValuePair<string, string>("EdiData", ediData),
                new KeyValuePair<string, string>("DomainData", domainData),
                new KeyValuePair<string, string>("UserId", userId)
            );

            // _repository.UpdateEventProcessRfcCompletion(id, processEnd, ediFileName, ediData, domainData, userId);
        }
    }
}
