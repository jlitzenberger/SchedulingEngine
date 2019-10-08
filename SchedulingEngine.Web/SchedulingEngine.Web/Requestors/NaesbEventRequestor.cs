using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SchedulingEngine.Web.Models;
using Nomination.Domain.Naesb;

namespace SchedulingEngine.Web.Requestors
{
    //public class NaesbEventRequestorSetting : BaseRequestorSettings
    //{
    //    public static NaesbEventRequestorSetting Get()
    //    {
    //        return new NaesbEventRequestorSetting()
    //        {
    //            Credentials = Properties.Settings.Default.PegasysCredentials,
    //            JsonSerializerSettings = new JsonSerializerSettings()
    //            {
    //                DefaultValueHandling = DefaultValueHandling.Ignore,
    //                TypeNameHandling = TypeNameHandling.None,
    //                NullValueHandling = NullValueHandling.Include,
    //                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
    //                Converters = { new Newtonsoft.Json.Converters.ExpandoObjectConverter() }
    //            },
    //            ApiHeaders = new Dictionary<string, string>
    //            {
    //                {"Content-Application-Name", Properties.Settings.Default.ContentApplicationName},
    //                {"Content-User-Id", Properties.Settings.Default.ContentUserId}
    //            }
    //        };
    //    }
    //}

    public class NaesbEventRequestor : BaseRequestor, IResourse<NaesbEvent>
    {
        public NaesbEventRequestor(IHttpClient client, NaesbEventRequestorSetting setting)
            : base(
                    client,
                    new BaseRequestorSettings
                    {
                        ApiHeaders = setting.ApiHeaders,
                        Credentials = setting.Credentials,
                        JsonSerializerSettings = setting.JsonSerializerSettings
                    }
                )
        {

        }
        public NaesbEvent Get(Uri uri)
        {
            return base.Get<NaesbEvent>(uri);
        }

        public HttpResponseMessage Post(Uri uri, object obj)
        {
            return base.Post(uri, obj);
        }

        public HttpResponseMessage Put(Uri uri, NaesbEvent obj)
        {
            return base.Put(uri, obj);
        }
    }
}