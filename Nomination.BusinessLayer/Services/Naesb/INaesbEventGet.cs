using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public interface INaesbEventGet
    {
        Nomination.Domain.Naesb.NaesbEvent Invoke(int id);
        Nomination.Domain.Naesb.NaesbEvent Invoke(string fileType, string pipeline, string utility, string cycle);
    }
}
