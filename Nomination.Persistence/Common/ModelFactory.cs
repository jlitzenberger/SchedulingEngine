using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.RequestForConfirmation;
using Nomination.Persistence.Naesb;
using Nomination.Domain.Naesb;

namespace Nomination.Persistence.Common
{
    public class ModelFactory
    {
        public string UserId { get; set; }

        public ModelFactory()
        {
            
        }
        public ModelFactory(string userId)
        {
            UserId = userId;
        }

        /////////////////////////////////////////////////////////////////////////////////////
        /// Map RequestForConfirmation DTO from database to Domain RequestForConfirmation
        /////////////////////////////////////////////////////////////////////////////////////
        public Nomination.Domain.RequestForConfirmation.RequestForConfirmation Map(Nomination.Persistence.RequestForConfirmation.Dto.RequestForConfirmation obj)
        {
            if (obj != null)
            {
                Nomination.Domain.RequestForConfirmation.RequestForConfirmation item =
                    new Nomination.Domain.RequestForConfirmation.RequestForConfirmation
                    {
                        PurchaseOrderNumber = obj.PurchaseOrderNumber,
                        GasDay = DateTime.ParseExact(obj.GasDay, "yyyyMMdd", null),
                        GasDayStart = obj.GasDayStart,
                        GasDayEnd = obj.GasDayEnd,
                        Cycle = obj.Cycle,
                        PartyIndentificaton = Map(obj.PartyIndentificaton),
                        Locations = obj.Locations.Select(Map).ToList()
                    };

                return item;
            }

            return null;
        }

        public Nomination.Domain.RequestForConfirmation.PartyIndentificaton Map(Nomination.Persistence.RequestForConfirmation.Dto.PartyIndentificaton obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.RequestForConfirmation.PartyIndentificaton
                {
                    PipelineEntity = obj.PipelineEntityId,
                    UtilityEntity =  obj.UtilityEntityId
                };
            }

            return null;
        }
        public Nomination.Domain.RequestForConfirmation.Location Map(Nomination.Persistence.RequestForConfirmation.Dto.Location obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.RequestForConfirmation.Location
                {
                    Id = obj.Id,
                    ContractNominations = obj.ContractNominations.Select(Map).ToList()
                };
            }

            return null;
        }
        public Nomination.Domain.RequestForConfirmation.ContractNomination Map(Nomination.Persistence.RequestForConfirmation.Dto.ContractNomination obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.RequestForConfirmation.ContractNomination
                {
                    Id = obj.Id,
                    Nominations = obj.Nominations.Select(Map).ToList()
                };
            }

            return null;
        }
        public Nomination.Domain.RequestForConfirmation.Nomination Map(Nomination.Persistence.RequestForConfirmation.Dto.Nomination obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.RequestForConfirmation.Nomination
                {
                    Id = obj.Id,
                    FlowIndicator = obj.FlowIndicator,
                    Quantity = obj.Quantity,
                    Stream = Map(obj.Stream),
                    NomsContractInfo = Map(obj.NomsContractInfo)
                };
            }

            return null;
        }
        public Nomination.Domain.RequestForConfirmation.Stream Map(Nomination.Persistence.RequestForConfirmation.Dto.Stream obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.RequestForConfirmation.Stream
                {
                    Direction = obj.Direction,
                    EntityId = obj.EntityId,
                    ContractId = obj.ContractId
                };
            }

            return null;
        }
        public Nomination.Domain.RequestForConfirmation.NomsContractInfo Map(Nomination.Persistence.RequestForConfirmation.Dto.NomsContractInfo obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.RequestForConfirmation.NomsContractInfo
                {
                    EntityId = obj.EntityId,
                    ContractId = obj.ContractId
                };
            }

            return null;
        }
        

        /////////////////////////////////////////////////////////////
        /// Naesb Event - tracks when defined events happened
        ///             - RFC's will use CycleStart and Cycle End
        ///             - CR's and OSQ's will NOT use CycleStart and Cycle End and  will process when received
        /////////////////////////////////////////////////////////////
        public Nomination.Domain.Naesb.NaesbEvent Map(tb_naesb_event obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.Naesb.NaesbEvent
                {
                    Id = Convert.ToInt32(obj.EventId),
                    FileType = obj.FileType,
                    Pipeline = obj.PipelineCd,
                    Utility = obj.CompanyCd,
                    Cycle = obj.CycleCd,
                    ProcessedTime = obj.ProcessedTime != null ? obj.ProcessedTime : (DateTime?)null,
                    CycleStart = obj.CycleStart,
                    CycleEnd = obj.CycleEnd,
                    OffSet = obj.OffSet,
                    On = obj.On_Off,
                    LastUpdateUserId = obj.row_lst_upd_userid
                };
            }

            return null;
        }
        public tb_naesb_event Map(Nomination.Domain.Naesb.NaesbEvent obj)
        {
            if (obj != null)
            {
                return new tb_naesb_event
                {
                   EventId = obj.Id,
                   FileType = obj.FileType,
                   PipelineCd = new NaesbPipelineRepository().GetByPipelineEntityID(obj.Pipeline)?.Pipeline,
                   CompanyCd = new NaesbUtilityRepository().GetByUtilityEntityId(obj.Utility)?.Utility,
                   CycleCd = obj.Cycle,
                   ProcessedTime = obj.ProcessedTime,
                   CycleStart = obj.CycleStart,
                   CycleEnd = obj.CycleEnd,
                   OffSet = obj.OffSet,
                   On_Off = obj.On,
                   row_lst_upd_userid = obj.LastUpdateUserId
                };
            }

            return null;
        }

        /////////////////////////////////////////////////////////////
        /// Naesb Event Process - saves data about the defined event
        /////////////////////////////////////////////////////////////
        public Nomination.Domain.Naesb.NaesbEventProcess Map(tb_naesb_event_process obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.Naesb.NaesbEventProcess
                {
                    Id = Convert.ToInt32(obj.EventProcessId),
                    Type = obj.FileType,
                    GasDay = obj.GasDay,
                    Cycle = obj.CycleCd,
                    Pipeline = obj.PipelineCd,
                    Utility = obj.CompanyCd,
                    ProcessStart = obj.ProcessStart,
                    ProcessEnd = obj.ProcessEnd,
                    EdiFileName = obj.EDIFileName,
                    EdiData = obj.EDIData,
                    DomainData = obj.BUSData,
                    StackTrace = obj.StackTrace,
                    UserId = obj.row_lst_upd_userid
                };
            }

            return null;
        }
        public tb_naesb_event_process Map(Nomination.Domain.Naesb.NaesbEventProcess obj)
        {
            if (obj != null)
            {
                return new tb_naesb_event_process
                    {
                        EventProcessId = obj.Id,
                        FileType = obj.Type,
                        GasDay = obj.GasDay ?? default(DateTime),
                        CycleCd = obj.Cycle,
                        PipelineCd = obj.Pipeline,
                        CompanyCd = obj.Utility,
                        ProcessStart = obj.ProcessStart,
                        ProcessEnd = obj.ProcessEnd,
                        EDIFileName = obj.EdiFileName,
                        EDIData = obj.EdiData,
                        BUSData = obj.DomainData,
                        StackTrace = obj.StackTrace,
                        row_lst_upd_userid = obj.UserId
                    };
            }

            return null;
        }

        //////////////////////////////////////////////////////////////////////
        /// Naesb Event Monitor - determines when events are past due
        //////////////////////////////////////////////////////////////////////
        public Nomination.Domain.Naesb.NaesbEventMonitor Map(tb_naesb_event_monitor obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.Naesb.NaesbEventMonitor
                {
                    Id = Convert.ToInt32(obj.Id),
                    Pipeline = obj.Pipeline,
                    Utility = obj.Utility,
                    EventType = obj.EventType,
                    Cycle = obj.Cycle,
                    SortSeq = obj.SortSeq,
                    EventTypeDescription = obj.EventTypeDescription,
                    CycleDescription = obj.CycleDescription,
                    EventMonitorTime = obj.EventMonitorTime,
                    LastCheckedTime = obj.LastCheckedTime,
                    ActiveStart = obj.ActiveStart,
                    ActiveEnd = obj.ActiveEnd
                };
            }

            return null;
        }
        public tb_naesb_event_monitor Map(Nomination.Domain.Naesb.NaesbEventMonitor obj)
        {
            if (obj != null)
            {
                return new tb_naesb_event_monitor
                {
                    Id = obj.Id,
                    Pipeline = obj.Pipeline,
                    Utility = obj.Utility,
                    EventType = obj.EventType,
                    Cycle = obj.Cycle,
                    SortSeq = obj.SortSeq,
                    EventTypeDescription = obj.EventTypeDescription,
                    CycleDescription = obj.CycleDescription,
                    EventMonitorTime = obj.EventMonitorTime,
                    LastCheckedTime = obj.LastCheckedTime,
                    ActiveStart = obj.ActiveStart,
                    ActiveEnd = obj.ActiveEnd
                };
            }

            return null;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// Naesb Event Notification - actual notifications of events to be distributed
        /////////////////////////////////////////////////////////////////////////////////
        public Nomination.Domain.Naesb.NaesbEventNotification Map(tb_naesb_event_notifications obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.Naesb.NaesbEventNotification
                {
                    Id = Convert.ToInt32(obj.Id),
                    Source = obj.Source,
                    To = obj.To,
                    From = obj.From,
                    Cc = obj.CC,
                    Priority = obj.Priority,
                    Subject = obj.Subject,
                    Body = obj.Body,
                    IsHtml = obj.Html,
                    ProcessedTime = obj.ProcessedTime,
                    GasDay = obj.GasDay,
                    Pipeline = obj.PipelineCd,
                    Utility = obj.PipelineCd,
                    Cycle = obj.CycleCd,
                    Ref1 = obj.Ref1,
                    Ref2 = obj.Ref2,
                    Ref3 = obj.Ref3,
                    LastUpdateUserId = obj.row_cr_userid
                };
            }

            return null;
        }
        public tb_naesb_event_notifications Map(Nomination.Domain.Naesb.NaesbEventNotification obj)
        {
            if (obj != null)
            {
                return new tb_naesb_event_notifications
                {
                    Id = obj.Id,
                    Source = obj.Source,
                    To = obj.To,
                    From = obj.From,
                    CC = obj.Cc,
                    Priority = obj.Priority,
                    Subject = obj.Subject,
                    Body = obj.Body,
                    Html = obj.IsHtml,
                    ProcessedTime = obj.ProcessedTime,
                    GasDay = obj.GasDay,
                    PipelineCd = obj.Pipeline,
                    CompanyCd = obj.Utility,
                    CycleCd = obj.Cycle,
                    Ref1 = obj.Ref1,
                    Ref2 = obj.Ref2,
                    Ref3 = obj.Ref3,
                    row_cr_userid = obj.LastUpdateUserId

                };
            }

            return null;
        }
        

        ///////////////////////////////////////////////////
        /// RequestForConfirmation
        ///////////////////////////////////////////////////
        public tb_naesb_transaction_master Map(Nomination.Domain.RequestForConfirmation.RequestForConfirmation obj)
        {
            if (obj != null)
            {
                return new tb_naesb_transaction_master
                {
                    FileType = "RFC",
                    //PipelineCd = new NaesbPipelineRepository().GetByPipelineEntityID(obj.PartyIndentificaton.PipelineEntityId)?.Pipeline,
                    //CompanyCd = new NaesbUtilityRepository().GetByUtilityEntityId(obj.PartyIndentificaton.UtilityEntityId)?.Utility,
                    PipelineCd = obj.PartyIndentificaton.PipelineEntity,
                    CompanyCd = obj.PartyIndentificaton.UtilityEntity,
                    GasDay = obj.GasDay,
                    CycleCd = obj.Cycle,
                    ConfirmingEntityId = obj.PartyIndentificaton.PipelineEntity,
                    UtilityEntityId = obj.PartyIndentificaton.UtilityEntity,
                    TransportationId = obj.PurchaseOrderNumber,
                    //TransportationId = null,
                    NomCycleStart = DateTime.ParseExact(obj.GasDayStart, "yyyyMMddHHmm", null),
                    NomcycleEnd = DateTime.ParseExact(obj.GasDayEnd, "yyyyMMddHHmm", null),
                    TransactionTime = DateTime.Now,
                    row_lst_upd_userid = UserId,
                    tb_naesb_transaction_detail = Map(obj.Locations)
                };
            }

            return null;
        }
        public List<tb_naesb_transaction_detail> Map(List<Nomination.Domain.RequestForConfirmation.Location> objs)
        {
            if (objs != null && objs.Count > 0)
            {
                List<tb_naesb_transaction_detail> ntds = new List<tb_naesb_transaction_detail>();

                foreach (var loc in objs)
                {
                    foreach (var cn in loc.ContractNominations)
                    {
                        foreach (var nom in cn.Nominations)
                        {
                            ntds.Add(
                                new tb_naesb_transaction_detail
                                {
                                    RecordType = "RFC",
                                    LocationCode = loc.Id,
                                    ConfirmationTrackingId = nom.Id,
                                    Quantity = int.Parse(nom.Quantity),
                                    SupplierEntityId = nom.NomsContractInfo.EntityId,
                                    ContractId = nom.NomsContractInfo.ContractId,
                                    ServiceRequesterId = nom.Stream.EntityId,
                                    PipelineContractId = nom.Stream.ContractId,
                                    Flow = nom.FlowIndicator,
                                    row_lst_upd_userid = UserId
                                }
                            );
                        }
                    }
                }

                return ntds;
            }

            return null;
        }
        public Nomination.Domain.RequestForConfirmation.RequestForConfirmation MapRfc(tb_naesb_transaction_master obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.RequestForConfirmation.RequestForConfirmation
                {
                    Cycle = obj.CycleCd,
                    GasDay = obj.GasDay,
                    GasDayStart = obj.NomCycleStart?.ToString("yyyyMMddHHmm"),
                    GasDayEnd = obj.NomcycleEnd?.ToString("yyyyMMddHHmm"),
                    PartyIndentificaton = new Domain.RequestForConfirmation.PartyIndentificaton
                    {
                        PipelineEntity = obj.ConfirmingEntityId,
                        UtilityEntity = obj.UtilityEntityId
                    },
                    PurchaseOrderNumber = obj.TransportationId,
                    Locations = MapRfc(obj.tb_naesb_transaction_detail)
                };
            }

            return null;
        }
        public List<Nomination.Domain.RequestForConfirmation.Location> MapRfc(ICollection<tb_naesb_transaction_detail> objs)
        {
            List<Nomination.Domain.RequestForConfirmation.Location> locations = new List<Nomination.Domain.RequestForConfirmation.Location>();

            if (objs != null && objs.Count > 0)
            {
                //group by LocationCode
                var locationsGroup = objs
                    .GroupBy(s => s.LocationCode)
                    .Select(g => new { Locations = g });

                foreach (var lg in locationsGroup)
                {
                    //group by ContractId
                    var contractNominationsGroup = lg.Locations
                        .GroupBy(s => s.ContractId)
                        .Select(g => new { ContractNominations = g });

                    Nomination.Domain.RequestForConfirmation.Location location = new Nomination.Domain.RequestForConfirmation.Location();
                    location.Id = lg.Locations.Key;
                    location.ContractNominations = new List<Domain.RequestForConfirmation.ContractNomination>();

                    foreach (var cng in contractNominationsGroup)
                    {
                        Nomination.Domain.RequestForConfirmation.ContractNomination contractNomination = new Nomination.Domain.RequestForConfirmation.ContractNomination();
                        contractNomination.Id = cng.ContractNominations.Key;
                        contractNomination.Nominations = new List<Nomination.Domain.RequestForConfirmation.Nomination>();

                        foreach (var nomination in cng.ContractNominations)
                        {
                            Nomination.Domain.RequestForConfirmation.Nomination nom = new Domain.RequestForConfirmation.Nomination();
                            nom.Id = nomination.ConfirmationTrackingId;
                            nom.Quantity = nomination.Quantity.ToString();
                            //nom.Unit = not currently stored.
                            nom.FlowIndicator = nomination.Flow;
                            nom.Stream = new Domain.RequestForConfirmation.Stream()
                            {
                                EntityId = nomination.ServiceRequesterId,
                                ContractId = nomination.PipelineContractId
                            };
                            nom.NomsContractInfo = new Domain.RequestForConfirmation.NomsContractInfo()
                            {
                                EntityId = nomination.SupplierEntityId,
                                ContractId = nomination.ContractId
                            };

                            contractNomination.Nominations.Add(nom);
                        }

                        location.ContractNominations.Add(contractNomination);

                    }

                    locations.Add(location);
                }
            }

            return locations;
        }

        ///////////////////////////////////////////////////
        /// ConfirmationResponse
        ///////////////////////////////////////////////////
        public tb_naesb_transaction_master Map(Nomination.Domain.ConfirmationResponse.ConfirmationResponse obj)
        {
            if (obj != null)
            {
                return new tb_naesb_transaction_master
                {
                    FileType = "CR",
                    //PipelineCd = new NaesbPipelineRepository().GetByPipelineEntityID(obj.PartyIndentificaton.PipelineEntityId)?.Pipeline,
                    //CompanyCd = new NaesbUtilityRepository().GetByUtilityEntityId(obj.PartyIndentificaton.UtilityEntityId)?.Utility,
                    PipelineCd = obj.PartyIndentificaton.PipelineEntity,
                    CompanyCd = obj.PartyIndentificaton.UtilityEntity,
                    GasDay = obj.GasDay,
                    CycleCd = obj.Cycle,
                    ConfirmingEntityId = obj.PartyIndentificaton.PipelineEntity,
                    UtilityEntityId = obj.PartyIndentificaton.UtilityEntity,
                    TransportationId = obj.PurchaseOrderNumber,
                    NomCycleStart = DateTime.ParseExact(obj.GasDayStart, "yyyyMMddHHmm", null),
                    NomcycleEnd = DateTime.ParseExact(obj.GasDayEnd, "yyyyMMddHHmm", null),
                    TransactionTime = DateTime.Now,
                    row_lst_upd_userid = UserId,
                    tb_naesb_transaction_detail = Map(obj.Locations)
                };
            }

            return null;
        }
        public List<tb_naesb_transaction_detail> Map(List<Nomination.Domain.ConfirmationResponse.Location> objs)
        {
            if (objs != null && objs.Count > 0)
            {
                List<tb_naesb_transaction_detail> ntds = new List<tb_naesb_transaction_detail>();

                foreach (var loc in objs)
                {
                    foreach (var cn in loc.ContractNominations)
                    {
                        foreach (var nom in cn.Nominations)
                        {
                            ntds.Add(
                                new tb_naesb_transaction_detail
                                {
                                    RecordType = "CR",
                                    LocationCode = loc.Id,
                                    ConfirmationTrackingId = nom.Id,
                                    Quantity = int.Parse(nom.Quantity),
                                    SupplierEntityId = nom.NomsContractInfo.EntityId,
                                    ContractId = nom.NomsContractInfo.ContractId,
                                    ServiceRequesterId = nom.Stream.EntityId,
                                    PipelineContractId = nom.Stream.ContractId,
                                    Flow = nom.FlowIndicator,
                                    SolicitedInd = nom.SolicitedIndicator,
                                    ReductionReason = nom.ReductionReason,
                                    row_lst_upd_userid = UserId
                                    //add units to DB????
                                }
                            );
                        }
                    }
                }

                return ntds;
            }

            return null;
        }
        public Nomination.Domain.ConfirmationResponse.ConfirmationResponse MapCr(tb_naesb_transaction_master obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.ConfirmationResponse.ConfirmationResponse
                {
                    Cycle = obj.CycleCd,
                    GasDay = obj.GasDay,
                    GasDayStart = obj.NomCycleStart?.ToString("yyyyMMddHHmm"),
                    GasDayEnd = obj.NomcycleEnd?.ToString("yyyyMMddHHmm"),
                    PartyIndentificaton =  new Domain.ConfirmationResponse.PartyIndentificaton
                    {
                        PipelineEntity = obj.ConfirmingEntityId,
                        UtilityEntity = obj.UtilityEntityId
                    },
                    PurchaseOrderNumber = obj.TransportationId,
                    Locations = MapCr(obj.tb_naesb_transaction_detail)
                };
            }

            return null;
        }
        public List<Nomination.Domain.ConfirmationResponse.Location> MapCr(ICollection<tb_naesb_transaction_detail> objs)
        {
            List<Nomination.Domain.ConfirmationResponse.Location> locations = new List<Nomination.Domain.ConfirmationResponse.Location>();

            if (objs != null && objs.Count > 0)
            {
                //group by LocationCode
                var locationsGroup = objs
                    .GroupBy(s => s.LocationCode)
                    .Select(g => new { Locations = g });

                foreach (var lg in locationsGroup)
                {
                    //group by ContractId
                    var contractNominationsGroup = lg.Locations
                        .GroupBy(s => s.ContractId)
                        .Select(g => new { ContractNominations = g });

                    Nomination.Domain.ConfirmationResponse.Location location = new Nomination.Domain.ConfirmationResponse.Location();
                    location.Id = lg.Locations.Key;
                    location.ContractNominations = new List<Domain.ConfirmationResponse.ContractNomination>();
                    
                    foreach (var cng in contractNominationsGroup)
                    {
                        Nomination.Domain.ConfirmationResponse.ContractNomination contractNomination = new Nomination.Domain.ConfirmationResponse.ContractNomination();
                        contractNomination.Id = cng.ContractNominations.Key;
                        contractNomination.Nominations = new List<Nomination.Domain.ConfirmationResponse.Nomination>();

                        foreach (var nomination in cng.ContractNominations)
                        {
                            Nomination.Domain.ConfirmationResponse.Nomination nom = new Domain.ConfirmationResponse.Nomination();
                            nom.Id = nomination.ConfirmationTrackingId;
                            nom.Quantity = nomination.Quantity.ToString();
                            //nom.Unit = not currently stored.
                            nom.FlowIndicator = nomination.Flow;
                            nom.SolicitedIndicator = nomination.SolicitedInd;
                            nom.ReductionReason = nomination.ReductionReason;
                            nom.Stream = new Domain.ConfirmationResponse.Stream()
                            {
                               EntityId = nomination.ServiceRequesterId,
                               ContractId = nomination.PipelineContractId
                            };
                            nom.NomsContractInfo = new Domain.ConfirmationResponse.NomsContractInfo()
                            {
                                EntityId = nomination.SupplierEntityId,
                                ContractId = nomination.ContractId
                            };

                            contractNomination.Nominations.Add(nom);
                        }

                        location.ContractNominations.Add(contractNomination);

                    }

                    locations.Add(location);
                }
            }

            return locations;
        }

        ///////////////////////////////////////////////////
        /// ScheduledQuantities
        ///////////////////////////////////////////////////
        public tb_naesb_transaction_master Map(Nomination.Domain.ScheduledQuantities.ScheduledQuantities obj)
        {
            if (obj != null)
            {
                return new tb_naesb_transaction_master
                {
                    FileType = "OSQ",
                    PipelineCd = obj.PartyIndentificaton.PipelineEntity,
                    CompanyCd = obj.PartyIndentificaton.UtilityEntity,
                    GasDay = obj.GasDay,
                    CycleCd = obj.Cycle,
                    ConfirmingEntityId = obj.PartyIndentificaton.PipelineEntity,
                    UtilityEntityId = obj.PartyIndentificaton.UtilityEntity,
                    TransportationId = obj.PurchaseOrderNumber,
                    NomCycleStart = DateTime.ParseExact(obj.GasDayStart, "yyyyMMddHHmm", null),
                    NomcycleEnd = DateTime.ParseExact(obj.GasDayEnd, "yyyyMMddHHmm", null),
                    TransactionTime = DateTime.Now,
                    row_lst_upd_userid = UserId,
                    tb_naesb_transaction_detail = Map(obj.Locations)
                };
            }

            return null;
        }
        public List<tb_naesb_transaction_detail> Map(List<Nomination.Domain.ScheduledQuantities.Location> objs)
        {
            if (objs != null && objs.Count > 0)
            {
                List<tb_naesb_transaction_detail> ntds = new List<tb_naesb_transaction_detail>();

                foreach (var loc in objs)
                {
                    foreach (var cn in loc.ContractNominations)
                    {
                        foreach (var nom in cn.Nominations)
                        {
                            ntds.Add(
                                new tb_naesb_transaction_detail
                                {
                                    RecordType = "OSQ",
                                    LocationCode = loc.Id,
                                    ConfirmationTrackingId = nom.Id,
                                    Quantity = int.Parse(nom.Quantity),
                                    //SupplierEntityId = nom?.Stream?.EntityId,
                                    //ContractId = nom?.Stream?.ContractId,
                                    //ServiceRequesterId = nom?.NomsContractInfo?.EntityId,
                                    //PipelineContractId = nom?.NomsContractInfo?.ContractId,
                                    SupplierEntityId = nom?.NomsContractInfo?.EntityId,
                                    ContractId = nom?.NomsContractInfo?.ContractId,
                                    ServiceRequesterId = nom?.Stream?.EntityId,
                                    PipelineContractId = nom?.Stream?.ContractId,
                                    Flow = nom.FlowIndicator,
                                    SolicitedInd = nom.SolicitedIndicator,
                                    ReductionReason = nom.ReductionReason,
                                    row_lst_upd_userid = UserId
                                    //add units to DB????
                                }
                            );
                        }
                    }
                }

                return ntds;
            }

            return null;
        }
        public Nomination.Domain.ScheduledQuantities.ScheduledQuantities MapOsq(tb_naesb_transaction_master obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.ScheduledQuantities.ScheduledQuantities
                {
                    Cycle = obj.CycleCd,
                    GasDay = obj.GasDay,
                    GasDayStart = obj.NomCycleStart?.ToString("yyyyMMddHHmm"),
                    GasDayEnd = obj.NomcycleEnd?.ToString("yyyyMMddHHmm"),
                    PartyIndentificaton = new Domain.ScheduledQuantities.PartyIndentificaton
                    {
                        PipelineEntity = obj.ConfirmingEntityId,
                        UtilityEntity = obj.UtilityEntityId
                    },
                    PurchaseOrderNumber = obj.TransportationId,
                    Locations = MapOsq(obj.tb_naesb_transaction_detail)
                };
            }

            return null;
        }
        public List<Nomination.Domain.ScheduledQuantities.Location> MapOsq(ICollection<tb_naesb_transaction_detail> objs)
        {
            List<Nomination.Domain.ScheduledQuantities.Location> locations = new List<Nomination.Domain.ScheduledQuantities.Location>();

            if (objs != null && objs.Count > 0)
            {
                //group by LocationCode
                var locationsGroup = objs
                    .GroupBy(s => s.LocationCode)
                    .Select(g => new { Locations = g });

                foreach (var lg in locationsGroup)
                {
                    //group by ContractId
                    var contractNominationsGroup = lg.Locations
                        .GroupBy(s => s.ContractId)
                        .Select(g => new { ContractNominations = g });

                    Nomination.Domain.ScheduledQuantities.Location location = new Nomination.Domain.ScheduledQuantities.Location();
                    location.Id = lg.Locations.Key;
                    location.ContractNominations = new List<Domain.ScheduledQuantities.ContractNomination>();

                    foreach (var cng in contractNominationsGroup)
                    {
                        Nomination.Domain.ScheduledQuantities.ContractNomination contractNomination = new Nomination.Domain.ScheduledQuantities.ContractNomination();
                        contractNomination.Id = cng.ContractNominations.Key;
                        contractNomination.Nominations = new List<Nomination.Domain.ScheduledQuantities.Nomination>();

                        foreach (var nomination in cng.ContractNominations)
                        {
                            Nomination.Domain.ScheduledQuantities.Nomination nom = new Domain.ScheduledQuantities.Nomination();
                            nom.Id = nomination.ConfirmationTrackingId;
                            nom.Quantity = nomination.Quantity.ToString();
                            //nom.Unit = not currently stored.
                            nom.FlowIndicator = nomination.Flow;
                            nom.SolicitedIndicator = nomination.SolicitedInd;
                            nom.ReductionReason = nomination.ReductionReason;
                            nom.Stream = new Domain.ScheduledQuantities.Stream()
                            {
                                EntityId = nomination.SupplierEntityId,
                                ContractId = nomination.ContractId
                            };
                            nom.NomsContractInfo = new Domain.ScheduledQuantities.NomsContractInfo()
                            {
                                EntityId = nomination.ServiceRequesterId,
                                ContractId = nomination.PipelineContractId
                            };

                            contractNomination.Nominations.Add(nom);
                        }

                        location.ContractNominations.Add(contractNomination);

                    }

                    locations.Add(location);
                }
            }

            return locations;
        }

        //////////////////////////////////////////////
        public NaesbTransaction Map(tb_naesb_transaction_master obj)
        {
            if (obj != null)
            {
                return new NaesbTransaction
                {
                    Id = Convert.ToInt32(obj.TransMasterId),
                    FileType = obj.FileType,
                    Pipeline = obj.PipelineCd,
                    Company = obj.PipelineCd,
                    GasDay = obj.GasDay,
                    Cycle = obj.CycleCd,
                    ConfirmingEntityId = obj.ConfirmingEntityId,
                    UtiltiyEntityId = obj.UtilityEntityId,
                    TransportationId = obj.TransportationId,
                    CycleStart = obj.NomCycleStart,
                    CycleEnd = obj.NomcycleEnd,
                    TransactionTime = obj.TransactionTime,
                    UserId = obj.row_lst_upd_userid,
                    Details = Map(obj.tb_naesb_transaction_detail)
                };
}

            return null;
        }
        public List<NaesbTransactionDetail> Map(ICollection<tb_naesb_transaction_detail> objs)
        {
            if (objs != null && objs.Count > 0)
            {
                List<NaesbTransactionDetail> list = new List<NaesbTransactionDetail>();
                foreach (var item in objs)
                {
                    list.Add(
                        new NaesbTransactionDetail
                        {
                            Id = Convert.ToInt32(item.TransDetailId),
                            MasterId = Convert.ToInt32(item.TransMasterId),
                            RecordType = item.RecordType
                        }
                    );
                }

                return list;
            }

            return null;
        }

        public Nomination.Domain.Naesb.NaesbPipeline Map(tb_naesb_pipelines obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.Naesb.NaesbPipeline
                {
                    Pipeline = obj.PipelineCd,
                    PipelineDescription = obj.PipelineDesc,
                    PipelineEntityId = obj.PipelineEntityId,
                    PointCode = obj.PointCode,
                    PipelineId = obj.PipelineId,
                    LastUpdateTime = obj.row_lst_upd_dttm,
                    LastUpdateUserId = obj.row_lst_upd_userid
                };
            }

            return null;
        }
        public tb_naesb_pipelines Map(Nomination.Domain.Naesb.NaesbPipeline obj)
        {
            if (obj != null)
            {
                return new tb_naesb_pipelines
                {
                    PipelineCd = obj.Pipeline,
                    PipelineDesc = obj.PipelineDescription,
                    PipelineEntityId = obj.PipelineEntityId,
                    PointCode = obj.PointCode,
                    PipelineId = obj.PipelineId,
                    row_lst_upd_dttm = obj.LastUpdateTime,
                    row_lst_upd_userid = obj.LastUpdateUserId
                };
            }

            return null;
        }

        public Nomination.Domain.Naesb.NaesbUtility Map(tb_company obj)
        {
            if (obj != null)
            {
                return new Nomination.Domain.Naesb.NaesbUtility
                {
                    Utility = obj.company_code,
                    UtilityShortDescription = obj.company_short_desc,
                    UtilityName = obj.company_short_desc,
                    UtilityId = obj.company_id,
                    UtilityEntityId = obj.company_entity_id,
                    LastUpdateTime = obj.row_lst_upd_dttm,
                    LastUpdateUserId = obj.row_lst_upd_userid
                };
            }

            return null;
        }
        public tb_company Map(Nomination.Domain.Naesb.NaesbUtility obj)
        {
            if (obj != null)
            {
                return new tb_company
                {
                    company_code = obj.Utility,
                    company_short_desc = obj.UtilityShortDescription,
                    company_name = obj.UtilityName,
                    company_id = obj.UtilityId,
                    company_entity_id = obj.UtilityEntityId,
                    row_lst_upd_dttm = obj.LastUpdateTime,
                    row_lst_upd_userid = obj.LastUpdateUserId
                };
            }

            return null;
        }



    }
}
