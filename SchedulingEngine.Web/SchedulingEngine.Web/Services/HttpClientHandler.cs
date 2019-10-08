using SchedulingEngine.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingEngine.Web.Services
{
    public class HttpClientHandler : IHttpClient
    {
        private HttpClient _client = new HttpClient();
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestUri)
        {
            try
            {
                _client.DefaultRequestHeaders
                  .Accept
                  .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //return await _client.SendAsync(requestUri);
                return _client.SendAsync(requestUri).Result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public Task<HttpResponseMessage> PostAsync(HttpRequestMessage requestUri)
        {
            throw new NotImplementedException();

        }
    }
}