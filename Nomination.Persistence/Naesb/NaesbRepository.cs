using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces.Naesb;

namespace Nomination.Persistence.Naesb
{
    public class NaesbRepository : INaesbRepository
    {
        public Domain.RequestForConfirmation.Naesb.Header GetNaesbRequestForConfirmationHeader(Domain.RequestForConfirmation.RequestForConfirmation obj)
        {
            string pipeline = obj.PartyIndentificaton.PipelineEntity;
            string utility = obj.PartyIndentificaton.UtilityEntity;

            string partnerId = string.Empty;
            if (pipeline.ToUpper() == "ANR" && utility.ToUpper() == "MGU")
            {
                partnerId = "xxxxx";
            }
            if (pipeline.ToUpper() == "ANR" && utility.ToUpper() == "NSG")
            {
                partnerId = "xxxxx";
            }
            if (pipeline.ToUpper() == "ANR" && utility.ToUpper() == "PGL")
            {
                partnerId = "xxxxx";
            }
            if (pipeline.ToUpper() == "ANR" && utility.ToUpper() == "WPS")
            {
                partnerId = "xxxxx";
            }
            if (pipeline.ToUpper() == "GLGT" && utility.ToUpper() == "MRC")
            {
                partnerId = "xxxxx";
            }
            if (pipeline.ToUpper() == "NNG" && utility.ToUpper() == "MRC")
            {
                partnerId = "xxxxx";
            }
            if (pipeline.ToUpper() == "GRD" && utility.ToUpper() == "PGL")
            {
                partnerId = "xxxxx";
            }
            if (pipeline.ToUpper() == "GRD" && utility.ToUpper() == "WPS")
            {
                partnerId = "xxxxx";
            }
            if (pipeline.ToUpper() == "VIK" && utility.ToUpper() == "MRC")
            {
                partnerId = "xxxxx";
            }
            if (pipeline.ToUpper() == "NGPL" && utility.ToUpper() == "NSG")
            {
                partnerId = "xxxxx";
            }
            if (pipeline.ToUpper() == "NGPL" && utility.ToUpper() == "PGL")
            {
                partnerId = "xxxxx";
            }
            if (pipeline.ToUpper() == "VEC" && utility.ToUpper() == "PGL")
            {
                partnerId = "xxxxx";
            }

            return new Domain.RequestForConfirmation.Naesb.Header
            {
                PartnerId = partnerId,
                Standard = "X",
                Version = "005020",
                TransactionSet = "873",
                EnvironmentFlag = "T"
            };

        }

        public Domain.QuickResponse.Naesb.Header GetNaesbQuickResponseHeader(Domain.ConfirmationResponse.ConfirmationResponse obj)
        {
            string pipeline = obj.PartyIndentificaton.PipelineEntity;
            string utility = obj.PartyIndentificaton.UtilityEntity;

            string partnerId = string.Empty;
            if (pipeline.ToUpper() == "NGPL" && utility.ToUpper() == "PGL")
            {
                partnerId = "PGL_NGPCrQr";
            }
            if (pipeline.ToUpper() == "NGPL" && utility.ToUpper() == "NSG")
            {
                partnerId = "NSG_NGPCrQr";
            }

            return new Domain.QuickResponse.Naesb.Header
            {
                PartnerId = partnerId,
                Standard = "X",
                Version = "006010",
                TransactionSet = "874",
                EnvironmentFlag = "T"
            };
        }
    }
}
