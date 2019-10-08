using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.BusinessLayer.Interfaces
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestUri);

        Task<HttpResponseMessage> PostAsync(HttpRequestMessage requestUri);
    }
}
