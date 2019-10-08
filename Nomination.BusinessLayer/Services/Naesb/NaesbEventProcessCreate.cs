using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;
using Nomination.BusinessLayer.Interfaces;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventProcessCreate : INaesbEventProcessCreate
    {
        private readonly INaesbEventProcessRepository _repository;
        public NaesbEventProcessCreate(INaesbEventProcessRepository repository)
        {
            _repository = repository;
        }
        public int Invoke(NaesbEventProcess obj)
        {
            return _repository.Create(obj);
        }
    }
}
