using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbPipelineGetByPipeline : INaesbPipelineGetByPipeline
    {
        private readonly INaesbPipelineRepository _repository;
        public NaesbPipelineGetByPipeline(INaesbPipelineRepository repository)
        {
            _repository = repository;
        }
        public NaesbPipeline Invoke(string pipeline)
        {
            var obj = _repository.GetByPipeline(pipeline);
            return obj;
        }
    }
}
