using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;
using Nomination.Persistence.Common;
using Nomination.Persistence.Shared;

namespace Nomination.Persistence.Naesb
{
    public class NaesbUtilityRepository : Repository<tb_company>, INaesbUtilityRepository
    {
        public NaesbUtility GetByUtilityEntityId(string utilityEntityId)
        {
            var obj = base.GetAll()
                .Where(p =>
                    p.company_entity_id == utilityEntityId
                )
                .Select(new ModelFactory().Map)
                .FirstOrDefault();

            return obj;
        }
        public NaesbUtility GetByUtility(string utility)
        {
            var obj = base.GetAll()
                .Where(p =>
                    p.company_code == utility
                )
                .Select(new ModelFactory().Map)
                .FirstOrDefault();

            return obj;
        }
    }
}
