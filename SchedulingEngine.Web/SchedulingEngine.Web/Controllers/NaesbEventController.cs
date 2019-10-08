using Autofac;
using Nomination.Domain.Naesb;
using SchedulingEngine.Web.Models;
using SchedulingEngine.Web.Requestors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SchedulingEngine.Web.Controllers
{
    //public class NaesbEventModel
    //{
    //    public string RequestMethod;
    //    public string RequestUri;
    //}

    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class NaesbEventController : ApiController
    {
        public static IContainer _container;

        public NaesbEventController()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Services.HttpClientHandler>().As<IHttpClient>();

            builder.RegisterType<NaesbEventRequestor>().As<IResourse<NaesbEvent>>();
            builder.RegisterType<NaesbEventsRequestor>().As<IResourse<List<NaesbEvent>>>();
            builder.Register(context => NaesbEventRequestorSetting.Get());

            _container = builder.Build();
        }

        // GET: api/NeasbEvent
        //[AcceptVerbs("GET")]
        [Route("api/NaesbEvent")]
        public IEnumerable<NaesbEvent> Get()
        {

            //dynamic workRequest = _workRequestRequestor.Get(new Uri(BaseManagedWorkApiUri + ResolveCurlyBrackets(Properties.Settings.Default.ManagedWorkOrderApiResourse_WorkRequest, workPacket.WorkRequestId.ToString())));

            //var stuff = _container.Resolve<IResourse<NaesbEvent>>().Get(
            //    new Uri(ResolveCurlyBrackets(Properties.Settings.Default.PegasysHost + Properties.Settings.Default.PegasysApiResourse_RequestForConfirmation, "NGPL-PGL-20190625-EVE"))
            //);

            NaesbEvent naesbEvent = _container.Resolve<IResourse<NaesbEvent>>().Get(
                new Uri(ResolveCurlyBrackets("http://localhost:4955" + Properties.Settings.Default.PegasysApiResourse_NaesbEvent, "1000000005"))
            );
            NaesbEvent naesbEvent2 = _container.Resolve<IResourse<NaesbEvent>>().Get(
                new Uri(ResolveCurlyBrackets("http://localhost:4955" + Properties.Settings.Default.PegasysApiResourse_NaesbEvent, "1000000022"))
            );
            NaesbEvent naesbEvent3 = _container.Resolve<IResourse<NaesbEvent>>().Get(
                new Uri(ResolveCurlyBrackets("http://localhost:4955" + Properties.Settings.Default.PegasysApiResourse_NaesbEvent, "1000000031"))
            );
            NaesbEvent naesbEvent4 = _container.Resolve<IResourse<NaesbEvent>>().Get(
                new Uri(ResolveCurlyBrackets("http://localhost:4955" + Properties.Settings.Default.PegasysApiResourse_NaesbEvent, "1000000034"))
            );

            List<NaesbEvent> naesbEvents = new List<NaesbEvent>();
            naesbEvents.Add(naesbEvent);
            naesbEvents.Add(naesbEvent2);
            naesbEvents.Add(naesbEvent3);
            naesbEvents.Add(naesbEvent4);

            return naesbEvents;

        }

        // GET: api/NeasbEvent/5
        //[AcceptVerbs("GET")]
        [Route("api/NaesbEvent/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("api/NaesbEvent/GetByPipeline/{eventType?}/{pipeline?}/{cycle?}/{utility?}")]
        public IEnumerable<NaesbEvent> GetByPipeline(string eventType = null, string pipeline = null, string cycle = null, string utility = null)
        {
            //List<NaesbEvent> objs = _container.Resolve<IResourse<List<NaesbEvent>>>().Get(
            //    new Uri(ResolveCurlyBrackets("http://localhost:4955" + "/Nomination-Web-Api/api/naesb-events/get-by-pipeline/{eventType}/{pipeline}/{cycle}/{utility}", eventType, pipeline, cycle, utility))
            //);

            List<NaesbEvent> objs = _container.Resolve<IResourse<List<NaesbEvent>>>().Get(
                new Uri(ResolveCurlyBrackets("http://localhost:4955" + "/Nomination-Web-Api/api/naesb-events/get-by-pipeline?eventType={eventType}&pipeline={pipeline}&cycle={cycle}&utility={utility}", eventType, pipeline, cycle, utility))
            );
            
            return objs;
        }

        // POST: api/NeasbEvent
        [HttpOptions]
        [AcceptVerbs("POST", "OPTIONS")]
        [Route("api/NaesbEvent")]
        public IHttpActionResult Post([FromBody]NaesbEvent value)
        {
            return Created("", value);

            int id = 3;

            if (id > 0)
            {
                string location = Request.RequestUri + "/" + id;
                return Created(location, id);
            }

            return BadRequest();
        }

        // PUT: api/NeasbEvent/5
        //[HttpOptions]
        //[AcceptVerbs("PUT")]
        [Route("api/NaesbEvent/{id}")]
        public IHttpActionResult Put([FromBody]NaesbEvent value, [FromUri]string id)
        {
            var naesbEvent = _container.Resolve<IResourse<NaesbEvent>>().Put(
                new Uri(ResolveCurlyBrackets("http://localhost:4955" + Properties.Settings.Default.PegasysApiResourse_NaesbEvent, id)), value
            );


            return Ok(value);
        }

        //// DELETE: api/NeasbEvent/5
        //public void Delete(int id)
        //{
        //}
        private string ResolveCurlyBrackets(string curlyBrackets, params string[] list)
        {
            int i = 0;
            var pattern = @"{.*?}";
            var replaced = Regex.Replace(curlyBrackets, pattern,
                delegate (Match m)
                {
                    string match = list[i] != null ? list[i].ToString() : null;
                    i++;
                    return match;
                });

            return replaced;
        }
    }
}
