﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Domain.Naesb;

namespace Nomination.BusinessLayer.Services.Naesb
{
    public class NaesbUtilityGetByUtility : INaesbUtilityGetByUtility
    {
        private readonly INaesbUtilityRepository _repository;
        public NaesbUtilityGetByUtility(INaesbUtilityRepository repository)
        {
            _repository = repository;
        }
        public NaesbUtility Invoke(string utility)
        {
            var obj = _repository.GetByUtility(utility);
            return obj;
        }
    }
}
