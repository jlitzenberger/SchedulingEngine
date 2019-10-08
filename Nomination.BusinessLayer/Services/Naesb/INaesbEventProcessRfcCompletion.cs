using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public interface INaesbEventProcessRfcCompletion
    {
        void Invoke(int id, DateTime processEnd, string ediFileName, string ediData, string domainData, string userId);
    }
}
