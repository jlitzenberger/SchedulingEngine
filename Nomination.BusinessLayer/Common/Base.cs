using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Common
{
    public class Base
    {
        //private DataAccessLayer.Repositories.UnitOfWork _unitOfWork;
        private ModelFactory _factory;

        //protected DataAccessLayer.Repositories.UnitOfWork UnitOfWork
        //{
        //    get
        //    {
        //        if (_unitOfWork == null)
        //        {
        //            FieldOrderEntities context = new FieldOrderEntities();
        //            _unitOfWork = new DataAccessLayer.Repositories.UnitOfWork(context);
        //        }
        //        return _unitOfWork;
        //    }
        //}
        protected ModelFactory ModelFactory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = new ModelFactory();
                }
                return _factory;
            }
        }
    }
}
