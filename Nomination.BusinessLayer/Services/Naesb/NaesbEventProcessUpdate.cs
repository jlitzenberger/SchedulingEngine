using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventProcessUpdate : INaesbEventProcessUpdate
    {
        private readonly INaesbEventProcessRepository _repository;
        public NaesbEventProcessUpdate(INaesbEventProcessRepository repository)
        {
            _repository = repository;
        }
        public void Invoke(int id, NaesbEventProcess obj)
        {
            _repository.Update(id, obj);
        }

        public void Invoke(int id, params object[] obj)
        {
            _repository.Update(id, obj);
        }
    }
}
