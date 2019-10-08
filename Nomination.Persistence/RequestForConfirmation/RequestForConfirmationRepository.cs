using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Persistence.Shared;
using CS.Common.Utilities;
using Nomination.Persistence.Common;

namespace Nomination.Persistence.RequestForConfirmation
{
    public class RequestForConfirmationRepository : IRequestForConfirmationRepository
    {
        public RequestForConfirmationRepository() : base()
        {

        }

        public Nomination.Domain.RequestForConfirmation.RequestForConfirmation Get(string pipeline, string utility, DateTime gasDay, string cycle)
        {
            //get the xml data from DB
            var xml = GetXmlFromDb(pipeline, utility, gasDay, cycle);

            if (xml != string.Empty)
            {
                //deserialize xml to DTO RequestForConfirmation
                var obj = XmlTransformer.XmlDeserialize<Nomination.Persistence.RequestForConfirmation.Dto.RequestForConfirmation>(xml.ToString());

                //map DTO from Database to domain RequestForConfirmation
                Nomination.Domain.RequestForConfirmation.RequestForConfirmation rfc = new ModelFactory().Map(obj);

                return rfc;
            }

            return null;
        }
        //public Nomination.Domain.RequestForConfirmation.RequestForConfirmation Get(int id)
        //{
        //    var obj = base.Get(p =>
        //            p.TransMasterId == id
        //        )
        //        .Select(new ModelFactory().MapRfc)
        //        .FirstOrDefault();

        //    return obj;
        //}
        //public int Create(Nomination.Domain.RequestForConfirmation.RequestForConfirmation obj, string userId)
        //{
        //    //map business to entity framework
        //    tb_naesb_transaction_master entity = new ModelFactory(userId).Map(obj);

        //    //insert to database
        //    base.Add(entity);

        //    return Convert.ToInt32(entity.TransMasterId);
        //}
        private static string GetXmlFromDb(string pipeline, string utility, DateTime gasDay, string cycle)
        {
            var xmlOutput = new SqlParameter
            {
                ParameterName = "@xml_out",
                DbType = DbType.Xml,
                Direction = ParameterDirection.Output
            };

            var xmlResults = new PegasysContext().Database.SqlQuery<object>(
                "storedproc @PipelineCd, @CompanyCd, @GasDay, @Cycle, @xml_out out",
                new System.Data.SqlClient.SqlParameter("@PipelineCd", pipeline),
                new System.Data.SqlClient.SqlParameter("@CompanyCd", utility),
                new System.Data.SqlClient.SqlParameter("@GasDay", gasDay),
                new System.Data.SqlClient.SqlParameter("@Cycle", cycle),
                xmlOutput
            );

            xmlResults.FirstOrDefault();
            var xml = xmlOutput.Value.ToString();

            return xml;
        }
    }
}
