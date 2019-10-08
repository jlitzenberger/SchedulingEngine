using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public interface INaesbEventProcessError
    {
        void Invoke(int id, string type, string stacktrace);
    }
}
