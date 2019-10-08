using System;

namespace Nomination.Domain.Naesb
{
    public class NaesbError : Exception
    {
        public string ReasonCode { get; set; }
        public string Value { get; set; }
    }
}