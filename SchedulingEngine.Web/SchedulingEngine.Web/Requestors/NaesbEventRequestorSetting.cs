using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SchedulingEngine.Web.Models;
using Nomination.Domain.Naesb;

namespace SchedulingEngine.Web.Requestors
{
    public class NaesbEventRequestorSetting : BaseRequestorSettings
    {
        public static NaesbEventRequestorSetting Get()
        {
            return new NaesbEventRequestorSetting()
            {
                Credentials = Properties.Settings.Default.PegasysCredentials,
                JsonSerializerSettings = new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.None,
                    NullValueHandling = NullValueHandling.Include,
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                    Converters = { new Newtonsoft.Json.Converters.ExpandoObjectConverter() }
                },
                ApiHeaders = new Dictionary<string, string>
                {
                    {"Content-Application-Name", Properties.Settings.Default.ContentApplicationName},
                    {"Content-User-Id", Properties.Settings.Default.ContentUserId}
                }
            };
        }
    }
}