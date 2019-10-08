namespace Nomination.Persistence
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PegasysContext : DbContext
    {
        public PegasysContext()
            : base("name=PegasysContext")
        {
        }

        public virtual DbSet<tb_naesb_transaction_detail> tb_naesb_transaction_detail { get; set; }
        public virtual DbSet<tb_naesb_transaction_master> tb_naesb_transaction_master { get; set; }
        public virtual DbSet<tb_naesb_event> tb_naesb_event { get; set; }
        public virtual DbSet<tb_naesb_event_process> tb_naesb_event_process { get; set; }

        public virtual DbSet<tb_naesb_pipelines> tb_naesb_pipelines { get; set; }
        public virtual DbSet<tb_company> tb_company { get; set; }

        public virtual DbSet<tb_naesb_event_monitor> tb_naesb_event_monitor { get; set; }
        public virtual DbSet<tb_naesb_event_notifications> tb_naesb_event_notifications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.TransDetailId)
                .HasPrecision(13, 0);

            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.TransMasterId)
                .HasPrecision(13, 0);

            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.RecordType)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.LocationCode)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.ConfirmationTrackingId)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.SupplierEntityId)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.ServiceRequesterId)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.PipelineContractId)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.SolicitedInd)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.Flow)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.ReductionReason)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_detail>()
                .Property(e => e.row_lst_upd_userid)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_master>()
                .Property(e => e.TransMasterId)
                .HasPrecision(13, 0);

            modelBuilder.Entity<tb_naesb_transaction_master>()
                .Property(e => e.FileType)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_master>()
                .Property(e => e.PipelineCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_master>()
                .Property(e => e.CompanyCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_master>()
                .Property(e => e.CycleCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_master>()
                .Property(e => e.ConfirmingEntityId)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_master>()
                .Property(e => e.UtilityEntityId)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_master>()
                .Property(e => e.TransportationId)
                .IsUnicode(false);

            //modelBuilder.Entity<tb_naesb_transaction_master>()
            //    .Property(e => e.FileName)
            //    .IsUnicode(false);

            //modelBuilder.Entity<tb_naesb_transaction_master>()
            //    .Property(e => e.Stacktrace)
            //    .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_transaction_master>()
                .Property(e => e.row_lst_upd_userid)
                .IsFixedLength()
                .IsUnicode(false);


            modelBuilder.Entity<tb_naesb_event>()
                .Property(e => e.EventId)
                .HasPrecision(13, 0);

            modelBuilder.Entity<tb_naesb_event>()
                .Property(e => e.FileType)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event>()
                .Property(e => e.PipelineCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event>()
                .Property(e => e.CompanyCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event>()
                .Property(e => e.CycleCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event>()
                .Property(e => e.row_lst_upd_userid)
                .IsFixedLength()
                .IsUnicode(false);


            modelBuilder.Entity<tb_naesb_event_process>()
                .Property(e => e.EventProcessId)
                .HasPrecision(13, 0);

            modelBuilder.Entity<tb_naesb_event_process>()
                .Property(e => e.FileType)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_process>()
                .Property(e => e.CycleCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_process>()
                .Property(e => e.PipelineCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_process>()
                .Property(e => e.CompanyCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_process>()
                .Property(e => e.EDIFileName)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_process>()
                .Property(e => e.StackTrace)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_process>()
                .Property(e => e.row_lst_upd_userid)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_pipelines>()
                .Property(e => e.PipelineCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_pipelines>()
                .Property(e => e.PipelineDesc)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_pipelines>()
                .Property(e => e.PipelineEntityId)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_pipelines>()
                .Property(e => e.PointCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_pipelines>()
                .Property(e => e.row_lst_upd_userid)
                .IsFixedLength()
                .IsUnicode(false);


            modelBuilder.Entity<tb_company>()
                .Property(e => e.company_code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tb_company>()
                .Property(e => e.company_short_desc)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tb_company>()
                .Property(e => e.company_name)
                .IsUnicode(false);

            modelBuilder.Entity<tb_company>()
                .Property(e => e.company_entity_id)
                .IsUnicode(false);

            modelBuilder.Entity<tb_company>()
                .Property(e => e.row_lst_upd_userid)
                .IsFixedLength()
                .IsUnicode(false);


            modelBuilder.Entity<tb_naesb_event_monitor>()
                .Property(e => e.Id)
                .HasPrecision(13, 0);

            modelBuilder.Entity<tb_naesb_event_monitor>()
                .Property(e => e.Pipeline)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_monitor>()
                .Property(e => e.Utility)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_monitor>()
                .Property(e => e.EventType)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_monitor>()
                .Property(e => e.Cycle)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_monitor>()
                .Property(e => e.EventTypeDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_monitor>()
                .Property(e => e.CycleDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_monitor>()
                .Property(e => e.row_lst_upd_userid)
                .IsFixedLength()
                .IsUnicode(false);



            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.Id)
                .HasPrecision(13, 0);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.Source)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.To)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.From)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.CC)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.Priority)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.Subject)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.Body)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.PipelineCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.CompanyCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.CycleCd)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.Ref1)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.Ref2)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.Ref3)
                .IsUnicode(false);

            modelBuilder.Entity<tb_naesb_event_notifications>()
                .Property(e => e.row_cr_userid)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
