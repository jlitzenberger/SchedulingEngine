using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventProcessGet : INaesbEventProcessGet
    {
        private readonly INaesbEventProcessRepository _repository;
        public NaesbEventProcessGet(INaesbEventProcessRepository repository)
        {
            _repository = repository;
        }
        public NaesbEventProcess Invoke(int id)
        {
            return _repository.Get(id);
        }
        public List<NaesbEventProcess> Invoke(string fileType, DateTime gasday, string pipeline, string utility, string cycle)
        {
            return _repository.GetByIdentity(fileType, gasday, pipeline, utility, cycle);
        }
    }
}
