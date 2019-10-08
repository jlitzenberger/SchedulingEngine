using Nomination.Persistence.Naesb;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Persistence.Common;
using Nomination.Persistence.Shared;
using Nomination.Domain.Naesb;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;

namespace Nomination.Persistence.Naesb
{
    public class NaesbEventRepository : Repository<tb_naesb_event>, INaesbEventRepository
    {
        public NaesbEvent Get(int id)
        {
            var obj = base.Get(p => p.EventId == id)
                .Select(new ModelFactory().Map).FirstOrDefault();

            return obj;
        }

        //public List<NaesbEvent> Find(Expression<Func<tb_naesb_event, bool>> filter = null)
        //{
        //    var objs = 
        //        base.Find(filter)
        //            .ToList();

        //    return new List<NaesbEvent>() { new NaesbEvent { Cycle = "TIM"} };
        //}
        public List<NaesbEvent> Find(Expression<Func<NaesbEvent, bool>> filter = null)
        {
            //use automapper to map Expression from NaesbEvent to tb_naesb_event
            //https://docs.automapper.org/en/stable/Expression-Translation-(UseAsDataSource).html
            //https://stackoverflow.com/questions/53940242/automapper-expression-mapping-issue
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<NaesbEvent, tb_naesb_event>()
                    .ForMember(dest => dest.CompanyCd, conf => conf.MapFrom(source => source.Utility))
                    .ForMember(dest => dest.CycleCd, conf => conf.MapFrom(source => source.Cycle))
                    .ForMember(dest => dest.FileType, conf => conf.MapFrom(source => source.FileType))
                    .ForMember(dest => dest.PipelineCd, conf => conf.MapFrom(source => source.Pipeline));
                cfg.CreateMap<tb_naesb_event, NaesbEvent>()
                    .ForMember(source => source.Utility, conf => conf.MapFrom(dest => dest.CompanyCd))
                    .ForMember(source => source.Cycle, conf => conf.MapFrom(dest => dest.CycleCd))
                    .ForMember(source => source.FileType, conf => conf.MapFrom(dest => dest.FileType))
                    .ForMember(source => source.Pipeline, conf => conf.MapFrom(dest => dest.PipelineCd));
            });
            var mapper = config.CreateMapper();
            
            var mappedExpression = mapper.Map<Expression<Func<tb_naesb_event, bool>>>(filter);

            return base.Get(mappedExpression)
                .Select(new ModelFactory().Map)
                .ToList();
        }
        //public List<NaesbEvent> GetByPipeline(string eventType = null, string pipeline = null, string cycle = null, string utility = null)
        //{
        //    //var objs = base.Get(p =>
        //    //    p.PipelineCd == pipeline &&
        //    //    p.CycleCd == cycle)
        //    //  .Select(new ModelFactory().Map)
        //    //  .ToList();


        //    ParameterExpression pe = Expression.Parameter(typeof(tb_naesb_event), "p");
        //    BinaryExpression be = Expression.Equal(Expression.Constant(0, typeof(int)), Expression.Constant(0, typeof(int)));

        //    if (eventType != null)
        //    {
        //        be = Expression.AndAlso(
        //            be,
        //            Expression.Equal(Expression.Property(pe, "FileType"),
        //            Expression.Constant(eventType, typeof(string)))
        //        );
        //    }
        //    if (pipeline != null)
        //    {
        //        be = Expression.AndAlso(
        //            be,
        //            Expression.Equal(Expression.Property(pe, "PipelineCd"),
        //            Expression.Constant(pipeline, typeof(string)))
        //        );
        //    }
        //    if (cycle != null)
        //    {
        //        be = Expression.AndAlso(
        //            be,
        //            Expression.Equal(Expression.Property(pe, "CycleCd"),
        //            Expression.Constant(cycle, typeof(string)))
        //        );
        //    }
        //    if (utility != null)
        //    {
        //        be = Expression.AndAlso(
        //            be,
        //            Expression.Equal(Expression.Property(pe, "CompanyCd"),
        //            Expression.Constant(utility, typeof(string)))
        //        );
        //    }

        //    var ExpressionTree = Expression.Lambda<Func<tb_naesb_event, bool>>(be, new[] { pe });

        //    var objs = base.Get(ExpressionTree)
        //        .Select(new ModelFactory().Map)
        //        .ToList();

        //    return objs;
        //}
        public List<NaesbEvent> GetCyclesToProcess(DateTime date, string fileType)
        {
            var objs = base.Get(p =>
                     p.FileType == fileType &&
                     p.On_Off == true && (
                      p.CycleStart <= date.TimeOfDay && p.CycleEnd > date.TimeOfDay && p.ProcessedTime == null ||
                      p.CycleStart <= date.TimeOfDay && p.CycleEnd > date.TimeOfDay && DbFunctions.DiffDays(p.ProcessedTime, date) > 0)  // DbFunctions.DiffDays -> date1 > date2 the result will be < 0 and date1 < date2 the result will be > 0
                     )
                    .Select(new ModelFactory().Map)
                    .ToList();

            return objs;
        }
        public NaesbEvent GetByIdentity(string fileType, string pipeline, string utility, string cycle)
        {
            var obj = base.GetAll()
                .Where(p =>
                    p.FileType == fileType &&
                    p.PipelineCd == pipeline &&
                    p.CompanyCd == utility &&
                    p.CycleCd == cycle
                )
                .Select(new ModelFactory().Map)
                .SingleOrDefault();

            return obj;
        }
        public void Change(int id, NaesbEvent obj)
        {
            obj.Id = id;
            var entity = new Nomination.Persistence.Common.ModelFactory().Map(obj);

            base.Change(entity);
        }
        public void Update(int id, params object[] list)
        {
            var entity = base.Get(id);

            foreach (object kvp in list)
            {
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Type")
                {
                    entity.FileType = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Cycle")
                {
                    entity.CycleCd = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Pipeline")
                {
                    entity.PipelineCd = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Utility")
                {
                    entity.CompanyCd = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, DateTime?> && ((KeyValuePair<string, DateTime?>)kvp).Key == "ProcessedTime")
                {
                    entity.ProcessedTime = ((KeyValuePair<string, DateTime?>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, TimeSpan?> && ((KeyValuePair<string, TimeSpan?>)kvp).Key == "CycleStart")
                {
                    entity.CycleStart = ((KeyValuePair<string, TimeSpan?>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, TimeSpan?> && ((KeyValuePair<string, TimeSpan?>)kvp).Key == "CycleEnd")
                {
                    entity.CycleEnd = ((KeyValuePair<string, TimeSpan?>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, short> && ((KeyValuePair<string, short>)kvp).Key == "OffSet")
                {
                    entity.OffSet = ((KeyValuePair<string, short>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, bool?> && ((KeyValuePair<string, bool?>)kvp).Key == "On")
                {
                    entity.On_Off = ((KeyValuePair<string, bool?>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "UserId")
                {
                    entity.row_lst_upd_userid = ((KeyValuePair<string, string>)kvp).Value;
                }
            }

            base.Change(entity);
        }

    }
}