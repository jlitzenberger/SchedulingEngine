using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventGet : INaesbEventGet
    {
        private readonly INaesbEventRepository _repository;
        public NaesbEventGet(INaesbEventRepository repository)
        {
            _repository = repository;
        }
        public NaesbEvent Invoke(int id)
        {
            return _repository.Get(id);
        }
        public NaesbEvent Invoke(string fileType, string pipeline, string utility, string cycle)
        {
            return _repository.GetByIdentity(fileType, pipeline, utility, cycle);
        }
    }
}
