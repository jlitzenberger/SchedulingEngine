using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.Domain.QuickResponse.Naesb;

namespace Nomination.BusinessLayer.Services.QuickResponse.Naesb
{
    public class NaesbQuickResponseHeaderGet : INaesbQuickResponseHeaderGet
    {
        private readonly INaesbRepository _repository;

        public NaesbQuickResponseHeaderGet(INaesbRepository repository)
        {
            _repository = repository;
        }
        public Header Invoke(Domain.ConfirmationResponse.ConfirmationResponse obj)
        {
            return _repository.GetNaesbQuickResponseHeader(obj);
        }
    }
}
