using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbUtilityGet : INaesbUtilityGet
    {
        private readonly INaesbUtilityRepository _repository;
        public NaesbUtilityGet(INaesbUtilityRepository repository)
        {
            _repository = repository;
        }
        public NaesbUtility Invoke(string utilityEntityId)
        {
            return _repository.GetByUtilityEntityId(utilityEntityId);
        }
    }

}
