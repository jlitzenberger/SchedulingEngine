﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public interface INaesbEventProcessUpdate
    {
        void Invoke(int id, NaesbEventProcess obj);
        void Invoke(int id, params object[] obj);
    }
}