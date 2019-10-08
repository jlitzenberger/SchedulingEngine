using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;
using Nomination.Domain.ScheduledQuantities;
using Nomination.Persistence.Shared;
using Nomination.Persistence.Common;

namespace Nomination.Persistence.ScheduledQuantity
{
    public class ScheduledQuantityEventRepository : Repository<tb_naesb_transaction_master>, IScheduledQuantityEventRepository
    {
        public ScheduledQuantityEventRepository() : base()
        {

        }
        
        public List<Domain.ScheduledQuantities.ScheduledQuantities> Get(string pipeline, string utility, DateTime gasday, string cycle)
        {
            var obj = base.Get(p =>
                    p.FileType == "OSQ" &&
                    p.PipelineCd == pipeline &&
                    p.CompanyCd == utility &&
                    p.GasDay == gasday &&
                    p.CycleCd == cycle
                )
                .Select(new ModelFactory().MapOsq)
                .ToList();

            return obj;
        }
        public Domain.ScheduledQuantities.ScheduledQuantities Get(int id)
        {
            var obj = base.Get(p =>
                    p.TransMasterId == id
                )
                .Select(new ModelFactory().MapOsq)
                .FirstOrDefault();

            return obj;
        }
        public NaesbTransaction Get(string pipeline, string fileType, string trackingId)
        {
            var obj = base.GetAll()
                .Where(p => p.PipelineCd == pipeline && p.FileType == fileType)
                .SelectMany(p => p.tb_naesb_transaction_detail)
                .Where(c => c.ConfirmationTrackingId == trackingId)
                .FirstOrDefault();

            if (obj != null)
            {
                return new ModelFactory().Map(obj.tb_naesb_transaction_master);
            }

            return null;
        }
        public int Create(Nomination.Domain.ScheduledQuantities.ScheduledQuantities obj, string userId)
        {
            //map business to enitity framework
            tb_naesb_transaction_master entity = new ModelFactory(userId).Map(obj);

            //insert to database
            base.Add(entity);

            return Convert.ToInt32(entity.TransMasterId);
        }
        public void Process(string pipeline, string utility, DateTime gasDay, string cycle)
        {
            var results = new PegasysContext().Database.SqlQuery<object>(
                "storedProc @PipelineCd, @CompanyCd, @GasDay, @Cycle",
                new SqlParameter("@PipelineCd", pipeline),
                new SqlParameter("@CompanyCd", utility),
                new SqlParameter("@GasDay", gasDay),
                new SqlParameter("@Cycle", cycle)
            ).FirstOrDefault();
        }
    }
}