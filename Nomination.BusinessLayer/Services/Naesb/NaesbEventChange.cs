using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventChange : INaesbEventChange
    {
        private readonly INaesbEventRepository _repository;
        public NaesbEventChange(INaesbEventRepository repository)
        {
            _repository = repository;
        }
        public void Invoke(int id, NaesbEvent obj)
        {
            _repository.Change(id, obj);
        }
    }
}
