using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.Domain.Naesb;
using Nomination.Persistence.Common;
using Nomination.Persistence.Shared;

namespace Nomination.Persistence.Naesb
{
    public class NaesbPipelineRepository : Repository<tb_naesb_pipelines>, INaesbPipelineRepository
    {
        public NaesbPipeline GetByPipelineEntityID(string pipelineEntityId)
        {
            var obj = base.GetAll()
                .Where(p =>
                    p.PipelineEntityId == pipelineEntityId.ToString()
                )
                .Select(new ModelFactory().Map)
                .FirstOrDefault();

            return obj;
        }
        public NaesbPipeline GetByPipeline(string pipeline)
        {
            var obj = base.GetAll()
                .Where(p =>
                    p.PipelineCd == pipeline
                )
                .Select(new ModelFactory().Map)
                .FirstOrDefault();

            return obj;
        }
    }
}
