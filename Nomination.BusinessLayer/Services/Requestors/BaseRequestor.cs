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
    public class BaseRequestor
    {
        private IHttpClient HttpClient { get; set; }
        protected BaseRequestorSettings BaseRequestorSettings { get; set; }
        public BaseRequestor(IHttpClient httpClient, BaseRequestorSettings baseRequestorSettings)
        {
            HttpClient = httpClient;
            BaseRequestorSettings = baseRequestorSettings;
        }
        protected HttpRequestMessage BuildHttpRequestMessage(HttpMethod httpMethod, Uri uri, HttpContent content = null)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = uri,
                Content = content
            };
            foreach (var header in BaseRequestorSettings.ApiHeaders)
            {
                request.Headers.Add(header.Key, header.Value);
            }
            request.Headers.Add("Authorization", BaseRequestorSettings.Credentials);

            return request;
        }
        protected async Task<HttpResponseMessage> ProcessMessage(HttpRequestMessage message)
        {
            return await HttpClient.SendAsync(message);
        }
        protected T Get<T>(Uri uri)
        {
            var request = BuildHttpRequestMessage(HttpMethod.Get, uri);
            var response = ProcessMessage(request).Result;

            dynamic obj = null;
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                response.Content = new StringContent(json);

                obj = DeserializeHttpContent<T>(BaseRequestorSettings.JsonSerializerSettings, response.Content);
            }
            else
            {
                //throw new HttpResponseException(response);
            }

            return obj;
        }
        public HttpResponseMessage Post(Uri uri, object obj)
        {
            string json = JsonConvert.SerializeObject(obj, BaseRequestorSettings.JsonSerializerSettings);

            var request = BuildHttpRequestMessage(HttpMethod.Post, uri, new StringContent(json, Encoding.UTF8, "application/json"));
            var response = ProcessMessage(request).Result;

            if (!response.IsSuccessStatusCode)
            {
                //throw new HttpResponseException(response);
            }

            return response;
        }
        public HttpResponseMessage Put(Uri uri, object obj)
        {
            string json = JsonConvert.SerializeObject(obj, BaseRequestorSettings.JsonSerializerSettings);

            var request = BuildHttpRequestMessage(HttpMethod.Put, uri, new StringContent(json, Encoding.UTF8, "application/json"));
            var response = ProcessMessage(request).Result;

            if (!response.IsSuccessStatusCode)
            {
                //throw new HttpResponseException(response);
            }

            return response;
        }
        private dynamic DeserializeHttpContent<T>(JsonSerializerSettings settings, HttpContent content)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(content.ReadAsStringAsync().Result, typeof(T), settings);
        }
    }
}
