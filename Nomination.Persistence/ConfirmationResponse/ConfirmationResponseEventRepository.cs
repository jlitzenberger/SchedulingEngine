using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;
using Nomination.Persistence.Common;
using Nomination.Persistence.Shared;
using Nomination.Domain.RequestForConfirmation;

namespace Nomination.Persistence.ConfirmationResponse
{
    public class ConfirmationResponseEventRepository : Repository<tb_naesb_transaction_master>, IConfirmationResponseEventRepository
    {
        public ConfirmationResponseEventRepository() : base()
        {

        }

        public List<Domain.ConfirmationResponse.ConfirmationResponse> Get(string pipeline, string utility, DateTime gasday, string cycle)
        {
            var obj = base.Get(p =>
                    p.FileType == "CR" &&
                    p.PipelineCd == pipeline &&
                    p.CompanyCd == utility &&
                    p.GasDay == gasday &&
                    p.CycleCd == cycle
                )
                .Select(new ModelFactory().MapCr)
                .ToList();

            return obj;
        }
        public Domain.ConfirmationResponse.ConfirmationResponse Get(int id)
        {
            var obj = base.Get(p =>
                    p.TransMasterId == id
                )
                .Select(new ModelFactory().MapCr)
                .FirstOrDefault();

            return obj;
        }
        public NaesbTransaction Get(string pipeline, string fileType, string trackingId)
        {
            //var obj1 = base.GetAll()
            //    .Where(p => p.PipelineCd == pipeline && p.FileType == fileType)
            //    .SelectMany(p => p.tb_naesb_transaction_detail)
            //    .Where(c => c.ConfirmationTrackingId == trackingId)
            //    .Select(p => new NaesbTransaction { FileType = p.tb_naesb_transaction_master.CycleCd })
            //    .FirstOrDefault();
            
            var obj = base.GetAll()
                .Where(p => p.PipelineCd == pipeline && p.FileType == fileType)
                .SelectMany(p => p.tb_naesb_transaction_detail)
                .Where(c => c.ConfirmationTrackingId == trackingId)
                .FirstOrDefault();

            if (obj != null)
            {
                return new ModelFactory().Map(obj.tb_naesb_transaction_master);
            }


            //var obj = base.GetAll()
            //    .Where(p => p.PipelineCd == pipeline && p.FileType == fileType)
            //    .SelectMany(p => p.tb_naesb_transaction_detail)
            //    .Where(c => c.ConfirmationTrackingId == trackingId)
            //    .Select(p => new ModelFactory().Map(p.tb_naesb_transaction_master))
            //    .FirstOrDefault();

            return null;
        }
        public int Create(Nomination.Domain.ConfirmationResponse.ConfirmationResponse obj, string userId)
        {
            //map domain to enitity framework
            tb_naesb_transaction_master entity = new ModelFactory(userId).Map(obj);

            //insert to database
            base.Add(entity);

            return Convert.ToInt32(entity.TransMasterId);

            //call the stored proc to process
            //ProcessConfirmationResponse();
        }
        public void Process(string pipeline, string utility, DateTime gasDay, string cycle)
        {
            var results = new PegasysContext().Database.SqlQuery<object>(
                "storedproc @PipelineCd, @CompanyCd, @GasDay, @Cycle",
                new SqlParameter("@PipelineCd", pipeline),
                new SqlParameter("@CompanyCd", utility),
                new SqlParameter("@GasDay", gasDay),
                new SqlParameter("@Cycle", cycle)
            ).FirstOrDefault();
        }
    }
}
