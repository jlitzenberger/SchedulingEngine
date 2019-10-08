using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public interface INaesbEventProcessGet
    {
        NaesbEventProcess Invoke(int id);
        List<NaesbEventProcess> Invoke(string fileType, DateTime gasday, string pipeline, string utility, string cycle);
    }
}
