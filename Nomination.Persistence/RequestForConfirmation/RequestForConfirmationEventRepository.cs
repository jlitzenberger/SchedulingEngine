using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Persistence.Common;
using Nomination.Persistence.Shared;

namespace Nomination.Persistence.RequestForConfirmation
{
    public class RequestForConfirmationEventRepository : Repository<tb_naesb_transaction_master>, IRequestForConfirmationEventRepository
    {
        public RequestForConfirmationEventRepository()
        {
            
        }
        public List<Domain.RequestForConfirmation.RequestForConfirmation> Get(string pipeline, string utility, DateTime gasDay, string cycle)
        {
            var obj = base.Get(p =>
                    p.FileType == "RFC" &&
                    p.PipelineCd == pipeline &&
                    p.CompanyCd == utility &&
                    p.GasDay == gasDay &&
                    p.CycleCd == cycle
                )
                .Select(new ModelFactory().MapRfc)
                .ToList();

            return obj;
        }
        public Domain.RequestForConfirmation.RequestForConfirmation Get(int id)
        {
            var obj = base.Get(p =>
                    p.TransMasterId == id
                )
                .Select(new ModelFactory().MapRfc)
                .FirstOrDefault();

            return obj;
        }
        public int Create(Nomination.Domain.RequestForConfirmation.RequestForConfirmation obj, string userId)
        {
            //map business to entity framework
            tb_naesb_transaction_master entity = new ModelFactory(userId).Map(obj);

            //insert to database
            base.Add(entity);

            return Convert.ToInt32(entity.TransMasterId);
        }
    }
}
