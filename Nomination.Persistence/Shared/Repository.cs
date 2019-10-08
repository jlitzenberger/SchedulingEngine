using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Nomination.Domain.RequestForConfirmation;
using System.Data.Entity.Validation;
using System.Linq.Expressions;
using Nomination.BusinessLayer.Interfaces;

namespace Nomination.Persistence.Shared
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public PegasysContext _database;
        
        public Repository()
        {
            _database = new PegasysContext();
        }

        public virtual IList<T> Find(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _database.Set<T>().Where(filter);

            return query.ToList();
        }
        public virtual IQueryable<T> GetAll()
        {
            return _database.Set<T>();
        }
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _database.Set<T>().Where(predicate);
        }
        public T Get(params object[] keys)
        {
            return _database.Set<T>().Find(keys);
        }
        public void Add(T entity)
        {
            _database.Set<T>().Add(entity);

            //try
            //{
                _database.SaveChanges();
            //}
            //catch (DbEntityValidationException ex)
            //{
            //    var x = 1;
            //}

        }
        public void Change(T entity)
        {
            _database.Set<T>().Attach(entity);
            _database.Entry(entity).State = EntityState.Modified;

            _database.SaveChanges();
        }
        public void Remove(T entity)
        {
            _database.Set<T>().Remove(entity);
        }
    }
}
