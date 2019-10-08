using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbEventProcessError : INaesbEventProcessError
    {
        private readonly INaesbEventProcessRepository _repository;
        public NaesbEventProcessError(INaesbEventProcessRepository repository)
        {
            _repository = repository;
        }
        public void Invoke(int id, string type, string stacktrace)
        {
            _repository.Update(id,
                new KeyValuePair<string, string>("Type", type),
                new KeyValuePair<string, string>("StackTrace", stacktrace)
            );
        }
    }
}
