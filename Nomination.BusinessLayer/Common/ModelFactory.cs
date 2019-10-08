using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Services.RequestForConfirmation;
using Nomination.Domain.RequestForConfirmation.Naesb;

namespace Nomination.BusinessLayer.Common
{
    public class ModelFactory
    {
        private List<ConfirmationResponseLocationContractNomination> ReMap(Nomination.Domain.ConfirmationResponse.Naesb.NaesbConfirmationResponse obj)
        {
            //will modify by reference
            AddNomsContractInfo(obj);

            //////////////////////////////////////////////
            //TODO:  This works but i dont understand why...try to fix it
            List<ConfirmationResponseLocationContractNomination> lcns =
            (
                from nomination in obj.Nominations
                from nominationDetail in nomination.Location.NominationDetails
                from identifier in nominationDetail.Identifiers
                where identifier.EntityIdCode == "78"
                from identifierDetail in identifier.IdentifierDetails
                where identifierDetail.Qualifier == "KSR"
                group nominationDetail by new
                {
                    LocationCode = nomination.Location.Code,
                    ServiceRequesterContractId = identifierDetail.Code
                }
                into grp
                orderby grp.Key.LocationCode
                select new ConfirmationResponseLocationContractNomination
                {
                    LocationCode = grp.Key.LocationCode,
                    ContractNominations = new List<ConfirmationResponseContractNomination>
                    {
                        new ConfirmationResponseContractNomination
                        {
                            ServiceRequesterContractId = grp.Key.ServiceRequesterContractId,
                            Nominations = grp.ToList()
                        }
                    }
                }
            ).ToList();

            List<ConfirmationResponseLocationContractNomination> objs =
            (
                from locationContractNominations in lcns
                from contractNominations in locationContractNominations.ContractNominations
                group contractNominations by new
                {
                    LocationCode = locationContractNominations.LocationCode,
                }
                into grp
                select new ConfirmationResponseLocationContractNomination
                {
                    LocationCode = grp.Key.LocationCode,
                    ContractNominations = grp.ToList()
                }
            ).ToList();

            return objs;
        }
        public class ConfirmationResponseLocationContractNomination
        {
            public string LocationCode { get; set; }
            public List<ConfirmationResponseContractNomination> ContractNominations { get; set; }
        }
        public class ConfirmationResponseContractNomination
        {
            public string ServiceRequesterContractId { get; set; }
            public List<Nomination.Domain.ConfirmationResponse.Naesb.NominationDetail> Nominations { get; set; }
        }

        private List<ScheduledQuantityLocationContractNomination> ReMap(Nomination.Domain.ScheduledQuantities.Naesb.NaesbScheduledQuantities obj)
        {
            //will modify by reference
            AddNomsContractInfo(obj);

            //////////////////////////////////////////////
            //TODO:  This works but i dont understand why...try to fix it
            List<ScheduledQuantityLocationContractNomination> lcns =
            (
                from nomination in obj.Nominations
                from nominationDetail in nomination.Location.NominationDetails
                from identifier in nominationDetail.Identifiers
                where identifier.EntityIdCode == "78"
                from identifierDetail in identifier.IdentifierDetails
                where identifierDetail.Qualifier == "KSR"
                group nominationDetail by new
                {
                    LocationCode = nomination.Location.Code,
                    ServiceRequesterContractId = identifierDetail.Code
                }
                into grp
                orderby grp.Key.LocationCode
                select new ScheduledQuantityLocationContractNomination
                {
                    LocationCode = grp.Key.LocationCode,
                    ContractNominations = new List<ScheduledQuantityContractNomination>
                    {
                        new ScheduledQuantityContractNomination
                        {
                            ServiceRequesterContractId = grp.Key.ServiceRequesterContractId,
                            Nominations = grp.ToList()
                        }
                    }
                }
            ).ToList();

            List<ScheduledQuantityLocationContractNomination> objs =
            (
                from locationContractNominations in lcns
                from contractNominations in locationContractNominations.ContractNominations
                group contractNominations by new
                {
                    LocationCode = locationContractNominations.LocationCode,
                }
                into grp
                select new ScheduledQuantityLocationContractNomination
                {
                    LocationCode = grp.Key.LocationCode,
                    ContractNominations = grp.ToList()
                }
            ).ToList();

            return objs;
        }
        public class ScheduledQuantityLocationContractNomination
        {
            public string LocationCode { get; set; }
            public List<ScheduledQuantityContractNomination> ContractNominations { get; set; }
        }
        public class ScheduledQuantityContractNomination
        {
            public string ServiceRequesterContractId { get; set; }
            public List<Nomination.Domain.ScheduledQuantities.Naesb.NominationDetail> Nominations { get; set; }
        }

        ///////////////////////////////////////////////////
        /// NaesbQuickResponse
        ///////////////////////////////////////////////////
        //public Nomination.Domain.Naesb.QuickResponse.NaesbQuickResponse MapQr(Nomination.Domain.Naesb.ConfirmationResponse.NaesbConfirmationResponse obj)
        //{
        //    if (obj != null)
        //    {
        //        Nomination.Domain.Naesb.QuickResponse.NaesbQuickResponse item =
        //            new Nomination.Domain.Naesb.QuickResponse.NaesbQuickResponse
        //            {
        //                Header = new Nomination.Domain.Naesb.QuickResponse.Header
        //                {
        //                    PartnerId = obj.Header.,
        //                    Standard = hdr.Standard,
        //                    Version = hdr.Version,
        //                    TransactionSet = hdr.TransactionSet,
        //                    EnvironmentFlag = hdr.EnvironmentFlag
        //                },
        //                PurposeCode = "27",
        //                PurchaseOrderNumber = obj.Header.TransactionIdentifier,
        //                PartyIndentificaton = Map(obj.PartyIndentificaton)
        //            };

        //        return item;
        //    }

        //    return null;
        //}

        ///////////////////////////////////////////////////
        /// RequestForConfirmation
        ///////////////////////////////////////////////////
        public Nomination.Domain.RequestForConfirmation.Naesb.NaesbRequestForConfirmation Map(Header hdr, Nomination.Domain.RequestForConfirmation.RequestForConfirmation obj)
        {
            if (obj != null)
            {
                Nomination.Domain.RequestForConfirmation.Naesb.NaesbRequestForConfirmation item =
                    new Nomination.Domain.RequestForConfirmation.Naesb.NaesbRequestForConfirmation
                    {
                        Header = new Nomination.Domain.RequestForConfirmation.Naesb.Header
                        {
                            PartnerId = hdr.PartnerId,
                            Standard = hdr.Standard,
                            Version = hdr.Version,
                            TransactionSet = hdr.TransactionSet,
                            EnvironmentFlag = hdr.EnvironmentFlag
                        },
                        PurposeCode = "00",
                        PurchaseOrderNumber = obj.PurchaseOrderNumber,
                        PartyIndentificaton = Map(obj.PartyIndentificaton),
                        Nominations =  Map(obj.GasDayStart, obj.GasDayEnd, obj.GasDay, obj.Cycle, obj.Locations)
                    };

                return item;
            }

            return null;
        }
        public Nomination.Domain.RequestForConfirmation.Naesb.PartyIndentificaton Map(Nomination.Domain.RequestForConfirmation.PartyIndentificaton obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.RequestForConfirmation.Naesb.PartyIndentificaton
                {
                    ConfirmingPartyDuns = obj.PipelineEntity,
                    UtilityDunsNumber = obj.UtilityEntity
                };
            }

            return null;
        }
        public List<Nomination.Domain.RequestForConfirmation.Naesb.Nomination> Map(string gasDayStart, string gasDayEnd, DateTime gasDay, string cycle, List<Nomination.Domain.RequestForConfirmation.Location> locations)
        {
            if (locations != null && locations.Count > 0)
            {
                List<Nomination.Domain.RequestForConfirmation.Naesb.Nomination> nominations = new List<Nomination.Domain.RequestForConfirmation.Naesb.Nomination>();

                foreach (var location in locations)
                {
                    foreach (var contractNomination in location.ContractNominations)
                    {
                        Nomination.Domain.RequestForConfirmation.Naesb.Nomination nomination = new Nomination.Domain.RequestForConfirmation.Naesb.Nomination();
                        nomination.BegToEndDate = gasDayStart.Trim() + "-" + gasDayEnd.Trim();
                        nomination.Cycle = cycle.Trim();
                        nomination.Location = location.Id;
                        nomination.NominationDetails = new List<Nomination.Domain.RequestForConfirmation.Naesb.NominationDetail>();
                        nomination.NomsContractNumber = contractNomination.Id;

                        foreach (var contractNominationDetail in contractNomination.Nominations)
                        {
                            nomination.NominationDetails.Add(Map(contractNominationDetail));
                        }

                        nominations.Add(nomination);
                    }
                }

                return nominations;
            }

            return null;
        }
        public Nomination.Domain.RequestForConfirmation.Naesb.NominationDetail Map(Nomination.Domain.RequestForConfirmation.Nomination obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.RequestForConfirmation.Naesb.NominationDetail
                {
                    NominationId = obj.Id,
                    Quantity = obj.Quantity,
                    FlowIndicator = obj.FlowIndicator,
                    UpstreamEntity = obj.Stream.Direction == "UP" ? obj.Stream?.EntityId : null,
                    UpstreamContractNumber = obj.Stream.Direction == "UP" ? obj.Stream?.ContractId : null,
                    UpstreamPackage = obj.Stream.Direction == "UP" ? obj.Stream?.PackageId : null,
                    DownstreamEntity = obj.Stream.Direction == "DOWN" ? obj.Stream?.EntityId : null,
                    DownstreamContractNumber = obj.Stream.Direction == "DOWN" ? obj.Stream?.ContractId : null,
                    DownstreamPackage = obj.Stream.Direction == "DOWN" ? obj.Stream?.PackageId : null,
                    NomsContractInfo = Map(obj.NomsContractInfo)
                };
            }

            return null;
        }
        public Nomination.Domain.RequestForConfirmation.Naesb.NomsContractInfo Map(Nomination.Domain.RequestForConfirmation.NomsContractInfo obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.RequestForConfirmation.Naesb.NomsContractInfo
                {
                    EntityId = obj.EntityId,
                    ContractId = obj.ContractId
                };
            }

            return null;
        }
        
        ///////////////////////////////////////////////////
        /// ConfirmationResponse
        ///////////////////////////////////////////////////
        public Nomination.Domain.ConfirmationResponse.ConfirmationResponse Map(Nomination.Domain.ConfirmationResponse.Naesb.NaesbConfirmationResponse obj)
        {
            if (obj != null)
            {
                //all NominationDetails with criteria -- WORKING!!
                List<ConfirmationResponseLocationContractNomination> locationContractNominations = ReMap(obj);

                Nomination.Domain.ConfirmationResponse.ConfirmationResponse item =
                    new Nomination.Domain.ConfirmationResponse.ConfirmationResponse
                    {
                        PurchaseOrderNumber = obj.Header.TransactionIdentifier,
                        GasDayStart = GasDayStart(obj),
                        GasDayEnd = GasDayEnd(obj),
                        GasDay = GasDay(obj),
                        Cycle = obj.Nominations[0]?.Cycle?.Indicator,
                        PartyIndentificaton = Map(obj.ConfirmingParties),
                        Locations = locationContractNominations.Select(Map).ToList()
                    };

                return item;
            }

            return null;
        }

        //this function will add NomsContractInfo nodes when null...by reference
        private void AddNomsContractInfo(Nomination.Domain.ConfirmationResponse.Naesb.NaesbConfirmationResponse obj)
        {
            foreach (var nom in obj.Nominations)
            {
                foreach (var detail in nom.Location.NominationDetails)
                {
                    foreach (var indentifier in detail.Identifiers)
                    {
                        if (indentifier.EntityIdCode == "78")
                        {
                            if (indentifier.IdentifierDetails != null)
                            {
                                //identifier of KSR does not exist
                                bool exists = indentifier.IdentifierDetails.Any(x => x.Qualifier == "KSR");
                                if (exists == false)
                                {
                                    indentifier.IdentifierDetails.Add(
                                        new Nomination.Domain.ConfirmationResponse.Naesb.IdentifierDetail
                                        {
                                            Qualifier = "KSR",
                                            Code = null
                                        }
                                    );
                                }
                            }
                            else
                            {
                                //add the entire list with one KSR
                                indentifier.IdentifierDetails = new List<Nomination.Domain.ConfirmationResponse.Naesb.IdentifierDetail>
                                {
                                    new Nomination.Domain.ConfirmationResponse.Naesb.IdentifierDetail
                                    {
                                        Qualifier = "KSR",
                                        Code = null
                                    }
                                };
                            }
                        }
                    }
                }
            }
        }
        private string GasDayStart(Nomination.Domain.ConfirmationResponse.Naesb.NaesbConfirmationResponse obj)
        {
            //GasDayStart = obj.Nominations[0].DateTimeFormatQualifier == "RDT" ? obj.Nominations[0].DateTimePeriod?.Split('-')[0] : null,

            if (obj.Nominations[0].DateTimeFormatQualifier == "RDT")
            {
                return obj.Nominations[0].DateTimePeriod?.Split('-')[0];
            }

            if (obj.Nominations[0].DateTimeFormatQualifier == "RD8")
            {
                return obj.Nominations[0].DateTimePeriod?.Split('-')[0] + "0900";
            }

            return null;
        }
        private string GasDayEnd(Nomination.Domain.ConfirmationResponse.Naesb.NaesbConfirmationResponse obj)
        {
            //GasDayEnd = obj.Nominations[0].DateTimeFormatQualifier == "RDT" ? obj.Nominations[0].DateTimePeriod?.Split('-')[1] : null,

            if (obj.Nominations[0].DateTimeFormatQualifier == "RDT")
            {
                return obj.Nominations[0].DateTimePeriod?.Split('-')[1];
            }

            if (obj.Nominations[0].DateTimeFormatQualifier == "RD8")
            {
                return obj.Nominations[0].DateTimePeriod?.Split('-')[1] + "0900";
            }

            return null;
        }
        private DateTime GasDay(Nomination.Domain.ConfirmationResponse.Naesb.NaesbConfirmationResponse obj)
        {
            //GasDay = obj.Nominations[0].DateTimeFormatQualifier == "RDT" ? DateTime.ParseExact(obj.Nominations[0].DateTimePeriod?.Split('-')[0].Substring(0, 8), "yyyyMMdd", null) : default(DateTime),

            if (obj.Nominations[0].DateTimeFormatQualifier == "RDT")
            {
                return DateTime.ParseExact(obj.Nominations[0].DateTimePeriod?.Split('-')[0].Substring(0, 8), "yyyyMMdd", null);
            }
            if (obj.Nominations[0].DateTimeFormatQualifier == "RD8")
            {
                return DateTime.ParseExact(obj.Nominations[0].DateTimePeriod?.Split('-')[0].Substring(0, 8), "yyyyMMdd", null);
            }

            return default(DateTime); ;
        }

        public Nomination.Domain.ConfirmationResponse.PartyIndentificaton Map(List<Nomination.Domain.ConfirmationResponse.Naesb.ConfirmingParty> objs)
        {
            if (objs != null && objs.Count > 0)
            {
                return new Nomination.Domain.ConfirmationResponse.PartyIndentificaton
                {
                    PipelineEntity = objs.Where(x => x.EntityIdentifierCode == "CNP").FirstOrDefault().ConfirmingId,
                    UtilityEntity = objs.Where(x => x.EntityIdentifierCode == "CNR").FirstOrDefault().ConfirmingId,
                };
            }

            return null;
        }
        public Nomination.Domain.ConfirmationResponse.Location Map(ConfirmationResponseLocationContractNomination obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.ConfirmationResponse.Location
                {
                    Id = obj.LocationCode,
                    ContractNominations = obj.ContractNominations.Select(Map).ToList()
                };
            }

            return null;
        }
        public Nomination.Domain.ConfirmationResponse.ContractNomination Map(ConfirmationResponseContractNomination obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.ConfirmationResponse.ContractNomination
                {
                    Id = obj.ServiceRequesterContractId,
                    Nominations = obj.Nominations.Select(Map).ToList()
                };
            }

            return null;
        }
        public Nomination.Domain.ConfirmationResponse.Nomination Map(Nomination.Domain.ConfirmationResponse.Naesb.NominationDetail obj)
        {
            if (obj != null)
            {
                string unit = string.Empty;
                if (obj != null && obj.Unit == "BZ")
                {
                    unit = "Million BTU's";
                }
                if (obj != null && obj.Unit == "G8")
                {
                    unit = "Gigacalories";
                }
                if (obj != null && obj.Unit == "GV")
                {
                    unit = "Gigajoules";
                }

                return new Nomination.Domain.ConfirmationResponse.Nomination
                {
                    Id = obj.ConfirmationTrackingId,
                    Quantity = obj.Quantity,
                    Unit = unit,
                    FlowIndicator = obj.AdditionalInformations.SingleOrDefault(x => x.Indentifier == "CFI")?.Code,
                    SolicitedIndicator = obj.AdditionalInformations.SingleOrDefault(x => x.Indentifier == "SUI")?.Code,
                    ReductionReason = obj.AdditionalInformations.SingleOrDefault(x => x.Indentifier == "RED")?.Code,
                    Stream = Map<Nomination.Domain.ConfirmationResponse.Stream>(obj.Identifiers.Where(x => x.EntityIdCode == "DW" || x.EntityIdCode == "US").ToList()),
                    NomsContractInfo = Map<Nomination.Domain.ConfirmationResponse.NomsContractInfo>(obj.Identifiers.Where(x => x.EntityIdCode == "78").ToList())
                };
            }

            return null;
        }
        public T Map<T>(List<Nomination.Domain.ConfirmationResponse.Naesb.Identifier> objs)
        {
            if (objs != null && objs.Count > 0)
            {
                if (typeof(T) == typeof(Nomination.Domain.ConfirmationResponse.Stream))
                {
                    string direction = string.Empty;
                    string id = string.Empty;
                    string contractId = string.Empty;
                    string packageId = string.Empty;

                    foreach (var obj in objs)
                    {
                        if (obj.EntityIdCode.ToUpper() == "DW" && obj.IdCodeQualifier == "1")
                        {
                            direction = "Down".ToUpper();
                            id = obj.Code;
                            contractId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "DT").SingleOrDefault()?.Code;
                            packageId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "PGD").SingleOrDefault()?.Code;
                        }
                        if (obj.EntityIdCode.ToUpper() == "US" && obj.IdCodeQualifier == "1")
                        {
                            direction = "Up".ToUpper();
                            id = obj.Code;
                            contractId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "UP").SingleOrDefault()?.Code;
                            packageId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "PKU").SingleOrDefault()?.Code;
                        }
                    }

                    Nomination.Domain.ConfirmationResponse.Stream item = new Nomination.Domain.ConfirmationResponse.Stream
                    {
                        Direction = direction,
                        EntityId = id,
                        ContractId = contractId,
                        PackageId = packageId
                    };

                    return (T)Convert.ChangeType(item, typeof(T));
                }

                if (typeof(T) == typeof(Nomination.Domain.ConfirmationResponse.NomsContractInfo))
                {
                    string id = string.Empty;
                    string contractId = string.Empty;

                    foreach (var obj in objs)
                    {
                        if (obj.EntityIdCode.ToUpper() == "78" && obj.IdCodeQualifier == "1")
                        {
                            id = obj.Code;
                            contractId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "KSR").SingleOrDefault()?.Code;
                        }
                    }

                    Nomination.Domain.ConfirmationResponse.NomsContractInfo item = new Nomination.Domain.ConfirmationResponse.NomsContractInfo
                    {
                        EntityId = id,
                        ContractId = contractId
                    };

                    return (T)Convert.ChangeType(item, typeof(T));
                }
            }

            return default(T);
        }
        
        ///////////////////////////////////////////////////
        /// ScheduledQuantities
        ///////////////////////////////////////////////////
        public Nomination.Domain.ScheduledQuantities.ScheduledQuantities Map(Nomination.Domain.ScheduledQuantities.Naesb.NaesbScheduledQuantities obj)
        {
            if (obj != null)
            {
                List<ScheduledQuantityLocationContractNomination> locationContractNominations = ReMap(obj);

                Nomination.Domain.ScheduledQuantities.ScheduledQuantities item =
                    new Nomination.Domain.ScheduledQuantities.ScheduledQuantities
                    {
                        PurchaseOrderNumber = obj.Header.TransactionIdentifier,
                        GasDayStart = GasDayStart(obj),
                        GasDayEnd = GasDayEnd(obj),
                        GasDay = GasDay(obj),
                        Cycle = obj.Nominations[0].Cycle.Indicator,
                        PartyIndentificaton = Map(obj.ConfirmingParties),
                        Locations = locationContractNominations.Select(Map).ToList()
                    };

                return item;
            }

            return null;
        }

        //this function will add NomsContractInfo nodes when null...by reference
        private void AddNomsContractInfo(Nomination.Domain.ScheduledQuantities.Naesb.NaesbScheduledQuantities obj)
        {
            foreach (var nom in obj.Nominations)
            {
                foreach (var detail in nom.Location.NominationDetails)
                {
                    foreach (var indentifier in detail.Identifiers)
                    {
                        if (indentifier.EntityIdCode == "78")
                        {
                            if (indentifier.IdentifierDetails != null)
                            {
                                //identifier of KSR does not exist
                                bool exists = indentifier.IdentifierDetails.Any(x => x.Qualifier == "KSR");
                                if (exists == false)
                                {
                                    indentifier.IdentifierDetails.Add(
                                        new Nomination.Domain.ScheduledQuantities.Naesb.IdentifierDetail
                                        {
                                            Qualifier = "KSR",
                                            Code = null
                                        }
                                    );
                                }
                            }
                            else
                            {
                                //add the entire list with one KSR
                                indentifier.IdentifierDetails = new List<Nomination.Domain.ScheduledQuantities.Naesb.IdentifierDetail>
                                {
                                    new Nomination.Domain.ScheduledQuantities.Naesb.IdentifierDetail
                                    {
                                        Qualifier = "KSR",
                                        Code = null
                                    }
                                };
                            }
                        }
                    }
                }
            }
        }
        private string GasDayStart(Nomination.Domain.ScheduledQuantities.Naesb.NaesbScheduledQuantities obj)
        {
            //GasDayStart = obj.Nominations[0].DateTimeFormatQualifier == "RDT" ? obj.Nominations[0].DateTimePeriod?.Split('-')[0] : null,

            if (obj.Nominations[0].DateTimeFormatQualifier == "RDT")
            {
                return obj.Nominations[0].DateTimePeriod?.Split('-')[0];
            }

            if (obj.Nominations[0].DateTimeFormatQualifier == "RD8")
            {
                return obj.Nominations[0].DateTimePeriod?.Split('-')[0] + "0900";
            }

            return null;
        }
        private string GasDayEnd(Nomination.Domain.ScheduledQuantities.Naesb.NaesbScheduledQuantities obj)
        {
            //GasDayEnd = obj.Nominations[0].DateTimeFormatQualifier == "RDT" ? obj.Nominations[0].DateTimePeriod?.Split('-')[1] : null,

            if (obj.Nominations[0].DateTimeFormatQualifier == "RDT")
            {
                return obj.Nominations[0].DateTimePeriod?.Split('-')[1];
            }

            if (obj.Nominations[0].DateTimeFormatQualifier == "RD8")
            {
                return obj.Nominations[0].DateTimePeriod?.Split('-')[1] + "0900";
            }

            return null;
        }
        private DateTime GasDay(Nomination.Domain.ScheduledQuantities.Naesb.NaesbScheduledQuantities obj)
        {
            //GasDay = obj.Nominations[0].DateTimeFormatQualifier == "RDT" ? DateTime.ParseExact(obj.Nominations[0].DateTimePeriod?.Split('-')[0].Substring(0, 8), "yyyyMMdd", null) : default(DateTime),

            if (obj.Nominations[0].DateTimeFormatQualifier == "RDT")
            {
                return DateTime.ParseExact(obj.Nominations[0].DateTimePeriod?.Split('-')[0].Substring(0, 8), "yyyyMMdd", null);
            }
            if (obj.Nominations[0].DateTimeFormatQualifier == "RD8")
            {
                return DateTime.ParseExact(obj.Nominations[0].DateTimePeriod?.Split('-')[0].Substring(0, 8), "yyyyMMdd", null);
            }

            return default(DateTime); ;
        }
        
        public Nomination.Domain.ScheduledQuantities.PartyIndentificaton Map(List<Nomination.Domain.ScheduledQuantities.Naesb.ConfirmingParty> objs)
        {
            if (objs != null && objs.Count > 0)
            {
                return new Nomination.Domain.ScheduledQuantities.PartyIndentificaton
                {
                    PipelineEntity = objs.Where(x => x.EntityIdentifierCode == "41").FirstOrDefault()?.ConfirmingId,
                    UtilityEntity = objs.Where(x => x.EntityIdentifierCode == "40").FirstOrDefault()?.ConfirmingId,
                };
            }

            return null;
        }
        public Nomination.Domain.ScheduledQuantities.Location Map(ScheduledQuantityLocationContractNomination obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.ScheduledQuantities.Location
                {
                    Id = obj.LocationCode,
                    ContractNominations = obj.ContractNominations.Select(Map).ToList()
                };
            }

            return null;
        }
        public Nomination.Domain.ScheduledQuantities.ContractNomination Map(ScheduledQuantityContractNomination obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.ScheduledQuantities.ContractNomination
                {
                    Id = obj.ServiceRequesterContractId,
                    Nominations = obj.Nominations.Select(Map).ToList()
                };
            }

            return null;
        }
        public Nomination.Domain.ScheduledQuantities.Nomination Map(Nomination.Domain.ScheduledQuantities.Naesb.NominationDetail obj)
        {
            if (obj != null)
            {
                string unit = string.Empty;
                if (obj != null && obj.Unit == "BZ")
                {
                    unit = "Million BTU's";
                }
                if (obj != null && obj.Unit == "G8")
                {
                    unit = "Gigacalories";
                }
                if (obj != null && obj.Unit == "GV")
                {
                    unit = "Gigajoules";
                }

                return new Nomination.Domain.ScheduledQuantities.Nomination
                {
                    Id = obj.ConfirmationTrackingId,
                    Quantity = obj.Quantity,
                    Unit = unit,
                    FlowIndicator = obj.AdditionalInformations.SingleOrDefault(x => x.Indentifier == "CFI")?.Code,
                    SolicitedIndicator = obj.AdditionalInformations.SingleOrDefault(x => x.Indentifier == "SUI")?.Code,
                    ReductionReason = obj.AdditionalInformations.SingleOrDefault(x => x.Indentifier == "RED")?.Code,
                    //Stream = Map<Nomination.Domain.ScheduledQuantities.Stream>(obj.Identifiers.Where(x => x.EntityIdCode == "DW" || x.EntityIdCode == "US").ToList()),
                    //NomsContractInfo = Map<Nomination.Domain.ScheduledQuantities.NomsContractInfo>(obj.Identifiers.Where(x => x.EntityIdCode == "78").ToList())
                    Stream = Map<Nomination.Domain.ScheduledQuantities.Stream>(obj.Identifiers),
                    NomsContractInfo = Map<Nomination.Domain.ScheduledQuantities.NomsContractInfo>(obj.Identifiers)
                };
            }

            return null;
        }
        public T Map<T>(List<Nomination.Domain.ScheduledQuantities.Naesb.Identifier> objs)
        {
            if (objs != null && objs.Count > 0)
            {
                if (typeof(T) == typeof(Nomination.Domain.ScheduledQuantities.Stream))
                {
                    string direction = string.Empty;
                    string id = string.Empty;
                    string contractId = string.Empty;
                    string packageId = string.Empty;

                    foreach (var obj in objs)
                    {
                        if (obj.EntityIdCode.ToUpper() == "78" && obj.IdCodeQualifier == "1")
                        {
                            id = obj.Code;
                            contractId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "KSR").SingleOrDefault()?.Code;
                        }

                        if (obj.EntityIdCode.ToUpper() == "DW" && obj.IdCodeQualifier == "1") //naesb downstream
                        {
                            direction = "Up".ToUpper(); //Flip the direction
                            //id = obj.Code;
                            //contractId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "DT").SingleOrDefault()?.Code;
                            packageId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "PGD").SingleOrDefault()?.Code;
                        }
                        if (obj.EntityIdCode.ToUpper() == "US" && obj.IdCodeQualifier == "1") //naesb upstream
                        {
                            direction = "Down".ToUpper(); //Flip the direction
                            //id = obj.Code;
                            //contractId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "UP").SingleOrDefault()?.Code;
                            packageId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "PKU").SingleOrDefault()?.Code;
                        }
                    }

                    Nomination.Domain.ScheduledQuantities.Stream item = new Nomination.Domain.ScheduledQuantities.Stream
                    {
                        Direction = direction,
                        EntityId = id,
                        ContractId = contractId,
                        PackageId = packageId
                    };

                    return (T)Convert.ChangeType(item, typeof(T));
                }

                if (typeof(T) == typeof(Nomination.Domain.ScheduledQuantities.NomsContractInfo))
                {
                    string id = string.Empty;
                    string contractId = string.Empty;

                    foreach (var obj in objs)
                    {
                        if (obj.EntityIdCode.ToUpper() == "DW" && obj.IdCodeQualifier == "1")
                        {
                            id = obj.Code;
                            contractId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "DT").SingleOrDefault()?.Code;
                        }
                        if (obj.EntityIdCode.ToUpper() == "US" && obj.IdCodeQualifier == "1")
                        {
                            id = obj.Code;
                            contractId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "UP").SingleOrDefault()?.Code;
                        }
                    }

                    Nomination.Domain.ScheduledQuantities.NomsContractInfo item = new Nomination.Domain.ScheduledQuantities.NomsContractInfo
                    {
                        EntityId = id,
                        ContractId = contractId
                    };

                    return (T)Convert.ChangeType(item, typeof(T));
                }
            }

            return default(T);
        }
        //public T Map<T>(List<Nomination.Domain.ScheduledQuantities.Naesb.Identifier> objs)
        //{
        //    if (objs != null && objs.Count > 0)
        //    {
        //        if (typeof(T) == typeof(Nomination.Domain.ScheduledQuantities.Stream))
        //        {
        //            string direction = string.Empty;
        //            string id = string.Empty;
        //            string contractId = string.Empty;
        //            string packageId = string.Empty;

        //            foreach (var obj in objs)
        //            {
        //                if (obj.EntityIdCode.ToUpper() == "DW" && obj.IdCodeQualifier == "1")
        //                {
        //                    direction = "Down".ToUpper();
        //                    id = obj.Code;
        //                    contractId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "DT").SingleOrDefault()?.Code;
        //                    packageId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "PGD").SingleOrDefault()?.Code;
        //                }
        //                if (obj.EntityIdCode.ToUpper() == "US" && obj.IdCodeQualifier == "1")
        //                {
        //                    direction = "Up".ToUpper();
        //                    id = obj.Code;
        //                    contractId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "UP").SingleOrDefault()?.Code;
        //                    packageId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "PKU").SingleOrDefault()?.Code;
        //                }
        //            }

        //            Nomination.Domain.ScheduledQuantities.Stream item = new Nomination.Domain.ScheduledQuantities.Stream
        //            {
        //                Direction = direction,
        //                EntityId = id,
        //                ContractId = contractId,
        //                PackageId = packageId
        //            };

        //            return (T)Convert.ChangeType(item, typeof(T));
        //        }

        //        if (typeof(T) == typeof(Nomination.Domain.ScheduledQuantities.NomsContractInfo))
        //        {
        //            string id = string.Empty;
        //            string contractId = string.Empty;

        //            foreach (var obj in objs)
        //            {
        //                if (obj.EntityIdCode.ToUpper() == "78" && obj.IdCodeQualifier == "1")
        //                {
        //                    id = obj.Code;
        //                    contractId = obj.IdentifierDetails.Where(x => x.Qualifier.ToUpper() == "KSR").SingleOrDefault()?.Code;
        //                }
        //            }

        //            Nomination.Domain.ScheduledQuantities.NomsContractInfo item = new Nomination.Domain.ScheduledQuantities.NomsContractInfo
        //            {
        //                EntityId = id,
        //                ContractId = contractId
        //            };

        //            return (T)Convert.ChangeType(item, typeof(T));
        //        }
        //    }

        //    return default(T);
        //}

    }
}
