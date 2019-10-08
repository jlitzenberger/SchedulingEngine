using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingEngine.Web.Models
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestUri);
        Task<HttpResponseMessage> PostAsync(HttpRequestMessage requestUri);
    }
}
