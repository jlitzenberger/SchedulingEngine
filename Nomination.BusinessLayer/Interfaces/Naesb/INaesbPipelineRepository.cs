using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Interfaces
{
    public interface INaesbPipelineRepository
    {
        NaesbPipeline GetByPipelineEntityID(string pipelineEntityId);
        NaesbPipeline GetByPipeline(string pipeline);
    }
}
