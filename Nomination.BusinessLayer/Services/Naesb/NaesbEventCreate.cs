using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventCreate : INaesbEventCreate
    {
        private readonly INaesbEventRepository _repository;
        public NaesbEventCreate(INaesbEventRepository repository)
        {
            _repository = repository;
        }

        public int Invoke(NaesbEvent obj)
        {
            throw new NotImplementedException();
        }
    }
}
