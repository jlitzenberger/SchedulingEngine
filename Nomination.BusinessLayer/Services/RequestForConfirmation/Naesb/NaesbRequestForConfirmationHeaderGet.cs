using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces.Naesb;

namespace Nomination.BusinessLayer.Services.RequestForConfirmation.Naesb
{
    public class NaesbRequestForConfirmationHeaderGet : INaesbRequestForConfirmationHeaderGet
    {
        private readonly INaesbRepository _repository;

        public NaesbRequestForConfirmationHeaderGet(INaesbRepository repository)
        {
            _repository = repository;
        }
        public Domain.RequestForConfirmation.Naesb.Header Invoke(Domain.RequestForConfirmation.RequestForConfirmation obj)
        {
            return _repository.GetNaesbRequestForConfirmationHeader(obj);
        }
    }
}
