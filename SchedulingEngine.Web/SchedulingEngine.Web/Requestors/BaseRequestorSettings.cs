using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SchedulingEngine.Web.Requestors
{
    public class BaseRequestorSettings
    {
        public string Credentials { get; set; }
        public JsonSerializerSettings JsonSerializerSettings { get; set; }
        public Dictionary<string, string> ApiHeaders { get; set; }

        public BaseRequestorSettings()
        {

        }
        public BaseRequestorSettings(string credentials, JsonSerializerSettings jsonSerializerSettings, Dictionary<string, string> apiHeaders)
        {
            Credentials = credentials;
            JsonSerializerSettings = jsonSerializerSettings;
            ApiHeaders = apiHeaders;
        }
    }
}