using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nomination.BusinessLayer.Interfaces;

namespace Nomination.BusinessLayer.Services.Requestors
{
    public class IncidentRequestorSetting : BaseRequestorSettings
    {
        public static IncidentRequestorSetting Get(string incidentUriCredentials, string contentApplicationName, string contentUserId)
        {
            return new IncidentRequestorSetting()
            {
                Credentials = incidentUriCredentials,
                JsonSerializerSettings = new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.None,
                    NullValueHandling = NullValueHandling.Include,
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                    Converters = {new Newtonsoft.Json.Converters.ExpandoObjectConverter()}
                },
                ApiHeaders = new Dictionary<string, string>
                {
                    {"Content-Application-Name", contentApplicationName},
                    {"Content-User-Id", contentUserId}
                }
            };
        }
    }

    public class IncidentRequestor : BaseRequestor, IResourse<Nomination.Domain.Incidents.Incident>
    {
        public IncidentRequestor(IHttpClient client, IncidentRequestorSetting setting)
            : base(client,
                new BaseRequestorSettings
                {
                    ApiHeaders = setting.ApiHeaders,
                    Credentials = setting.Credentials,
                    JsonSerializerSettings = setting.JsonSerializerSettings
                })
        {

        }

        public Nomination.Domain.Incidents.Incident Get(Uri uri)
        {
            return base.Get<Nomination.Domain.Incidents.Incident>(uri);
        }

        public HttpResponseMessage Post(Uri uri, object obj)
        {
            return base.Post(uri, obj);
        }

        public HttpResponseMessage Put(Uri uri, object obj)
        {
            throw new NotImplementedException();
        }
    }
}