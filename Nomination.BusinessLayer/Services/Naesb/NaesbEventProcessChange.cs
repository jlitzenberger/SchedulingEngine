using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventProcessChange : INaesbEventProcessChange
    {
        private readonly INaesbEventProcessRepository _repository;
        public NaesbEventProcessChange(INaesbEventProcessRepository repository)
        {
            _repository = repository;
        }
        public void Invoke(int id, NaesbEventProcess obj)
        {
            _repository.Change(id, obj);
        }
    }
}
