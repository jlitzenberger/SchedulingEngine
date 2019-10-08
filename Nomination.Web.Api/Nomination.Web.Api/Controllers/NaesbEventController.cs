using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autofac;
using Nomination.BusinessLayer.Services;
using Nomination.Persistence.Naesb;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.BusinessLayer.Interfaces;
using Nomination.Web.Api.Attributes;
using Swashbuckle.Swagger.Annotations;
using Nomination.Domain.Naesb;

namespace Nomination.Web.Api.Controllers
{
    public class NaesbEventController : ApiController
    {
        public static IContainer _container;

        public NaesbEventController()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => ServiceSettings.Get(
                    "xxxxx",
                    Nomination.Web.Api.Properties.Settings.Default.Environment
                )
            );

            builder.RegisterType<NaesbEventRepository>().As<INaesbEventRepository>();
            builder.RegisterType<NaesbEventGet>().As<INaesbEventGet>();
            builder.RegisterType<NaesbEventGetList>().As<INaesbEventGetList>();
            builder.RegisterType<NaesbEventUpdate>().As<INaesbEventUpdate>();

            _container = builder.Build();

        }
        // GET: api/NaesbEvent
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested naesb-events has successfully been returned", Type = typeof(IEnumerable<NaesbEvent>))]
        [SwaggerProduces("application/xml", "application/json")]
        [SwaggerOperation(Tags = new[] { "naesb-events" })]
        //[HttpGet]
        [Route("api/naesb-events")]
        public IHttpActionResult Get()
        {
            //List<NaesbEvent> list = _container.Resolve<INaesbEventGet>().Invoke(1);

            return Ok(new List<string>());
        }

        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested naesb-events has successfully been returned", Type = typeof(NaesbEvent))]
        [SwaggerProduces("application/xml", "application/json")]
        [SwaggerOperation(Tags = new[] { "naesb-events" })]
        //[HttpGet]
        [Route("api/naesb-events/{id}")]
        public IHttpActionResult Get(int id)
        {
            NaesbEvent naesbEvent = _container.Resolve<INaesbEventGet>().Invoke(id);

            if (naesbEvent != null)
            {
                return Ok(naesbEvent);
            }

            return NotFound();
        }

        [SwaggerOperation(Tags = new[] { "naesb-events" })]
        [Route("api/naesb-events/get-by-pipeline/{eventType?}/{pipeline?}/{cycle?}/{utility?}")]
        public IHttpActionResult GetByPipeline(string eventType = null, string pipeline = null, string cycle = null, string utility = null)
        {
            List<NaesbEvent> objs = _container.Resolve<INaesbEventGetList>().Invoke(eventType, pipeline, cycle, utility);

            if (objs != null)
            {
                return Ok(objs);
            }

            return NotFound();
        }

        //// POST: api/NaesbEvent
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT: api/NaesbEvent/5
        [SwaggerOperation(Tags = new[] { "naesb-events" })]
        [HttpPut]
        [Route("api/naesb-events/{id}")]
        public void Put(int id, [FromBody]NaesbEvent value)
        {
            _container.Resolve<INaesbEventUpdate>().Invoke(id, value);

            var x = Request;

        }

        //// DELETE: api/NaesbEvent/5
        //public void Delete(int id)
        //{
        //}
    }
}
