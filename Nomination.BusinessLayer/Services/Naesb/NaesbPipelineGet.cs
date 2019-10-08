using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbPipelineGet : INaesbPipelineGet
    {
        private readonly INaesbPipelineRepository _repository;
        public NaesbPipelineGet(INaesbPipelineRepository repository)
        {
            _repository = repository;
        }
        public NaesbPipeline Invoke(string pipelineEntityId)
        {
            var obj = _repository.GetByPipelineEntityID(pipelineEntityId);
            return obj;
        }
    }
}
