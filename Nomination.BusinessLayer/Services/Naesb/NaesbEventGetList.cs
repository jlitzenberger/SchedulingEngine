using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventGetList : INaesbEventGetList
    {
        private readonly INaesbEventRepository _repository;
        public NaesbEventGetList(INaesbEventRepository repository)
        {
            _repository = repository;
        }
        public List<NaesbEvent> Invoke(DateTime date, string fileType)
        {
            return _repository.GetCyclesToProcess(date, fileType);
        }
        public List<NaesbEvent> Invoke(string eventType = null, string pipeline = null, string cycle = null, string utility = null)
        {
            ParameterExpression pe = Expression.Parameter(typeof(NaesbEvent), "p");
            BinaryExpression be = Expression.Equal(Expression.Constant(0, typeof(int)), Expression.Constant(0, typeof(int)));

            if (eventType != null)
            {
                be = Expression.AndAlso(
                    be,
                    Expression.Equal(Expression.Property(pe, "FileType"),
                    Expression.Constant(eventType, typeof(string)))
                );
            }
            if (pipeline != null)
            {
                be = Expression.AndAlso(
                    be,
                    Expression.Equal(Expression.Property(pe, "Pipeline"),
                    Expression.Constant(pipeline, typeof(string)))
                );
            }
            if (cycle != null)
            {
                be = Expression.AndAlso(
                    be,
                    Expression.Equal(Expression.Property(pe, "Cycle"),
                    Expression.Constant(cycle, typeof(string)))
                );
            }
            if (utility != null)
            {
                be = Expression.AndAlso(
                    be,
                    Expression.Equal(Expression.Property(pe, "Utility"),
                    Expression.Constant(utility, typeof(string)))
                );
            }

            return _repository.Find(Expression.Lambda<Func<NaesbEvent, bool>>(be, new[] { pe }));
        }
    }
}
