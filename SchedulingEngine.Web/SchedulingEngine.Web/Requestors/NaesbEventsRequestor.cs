using SchedulingEngine.Web.Models;
using Nomination.Domain.Naesb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;

namespace SchedulingEngine.Web.Requestors
{
    public class NaesbEventsRequestor : BaseRequestor, IResourse<List<NaesbEvent>>
    {
        public NaesbEventsRequestor(IHttpClient client, NaesbEventRequestorSetting setting)
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
        public List<NaesbEvent> Get(Uri uri)
        {
            return base.Get<List<NaesbEvent>>(uri);
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