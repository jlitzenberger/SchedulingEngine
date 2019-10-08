using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T Get(params object[] keys);
        void Add(T entity);
        void Remove(T entity);
    }
}
