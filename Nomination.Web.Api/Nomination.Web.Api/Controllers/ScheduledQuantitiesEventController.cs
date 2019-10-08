using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autofac;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.BusinessLayer.Services.ScheduledQuantity.Event;
using Nomination.Domain.ScheduledQuantities;
using Nomination.Persistence.Naesb;
using Nomination.Persistence.ScheduledQuantity;
using Nomination.Web.Api.Attributes;
using Swashbuckle.Swagger.Annotations;

namespace Nomination.Web.Api.Controllers
{
    [SwaggerControllerOrder(6)]
    public class ScheduledQuantitiesEventController : ApiController
    {
        private static IContainer _container;
        public ScheduledQuantitiesEventController()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => ServiceSettings.Get(
                    "xxxxx",
                    Nomination.Web.Api.Properties.Settings.Default.Environment
                )
            );
            builder.RegisterType<ScheduledQuantityEventRepository>().As<IScheduledQuantityEventRepository>();
            builder.RegisterType<ScheduledQuantityEventGet>().As<IScheduledQuantityEventGet>();
            builder.RegisterType<ScheduledQuantityEventCreate>().As<IScheduledQuantityEventCreate>();
            builder.RegisterType<ScheduledQuantityEventProcess>().As<IScheduledQuantityEventProcess>();

            builder.RegisterType<NaesbEventRepository>().As<INaesbEventRepository>();
            builder.RegisterType<NaesbEventGet>().As<INaesbEventGet>();

            builder.RegisterType<NaesbPipelineRepository>().As<INaesbPipelineRepository>();
            builder.RegisterType<NaesbPipelineGet>().As<INaesbPipelineGet>();

            builder.RegisterType<NaesbUtilityRepository>().As<INaesbUtilityRepository>();
            builder.RegisterType<NaesbUtilityGet>().As<INaesbUtilityGet>();

            _container = builder.Build();
        }

        /// <summary>
        /// Get a list of operator-scheduled-quantities-events
        /// </summary> 
        /// <param name="pipeline_utility_gasday_cycle">Composite Id of the OSQ - {Pipeline}-{Utility}-{GasDay YYYYMMDD}-{Cycle}</param>
        /// <response code="200">operator-scheduled-quantities-events returned OK</response>
        /// <remarks>Returns a list of operator-scheduled-quantities-events.  NOTE: This will return data from the naesb_transaction_master/detail tables.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested operator-scheduled-quantities-events has successfully been returned", Type = typeof(IEnumerable<ScheduledQuantities>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested operator-scheduled-quantities-events does not exist")]
        [SwaggerProduces("application/xml", "application/json")]
        [SwaggerOperation(Tags = new[] { "operator-scheduled-quantities-events" })]
        [Route("api/operator-scheduled-quantities-events")]
        public IHttpActionResult Get(string pipeline_utility_gasday_cycle)
        {
            string[] key = pipeline_utility_gasday_cycle.Split('-');

            if (ValidateKey(key) == true)
            {
                var obj = _container.Resolve<IScheduledQuantityEventGet>()
                    .Invoke(
                          key[0]
                        , key[1]
                        , new DateTime(int.Parse(key[2].Substring(0, 4)), int.Parse(key[2].Substring(4, 2)), int.Parse(key[2].Substring(6, 2)))
                        , key[3]);

                if (obj != null)
                {
                    return Ok(obj);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get operator-scheduled-quantities-events
        /// </summary> 
        /// <param name="id">Naesb transaction master Id of the OSQ</param>
        /// <response code="200">operator-scheduled-quantities-events returned OK</response>
        /// <remarks>Returns a operator-scheduled-quantities-events.  NOTE: This will return data from the naesb_transaction_master/detail tables.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested operator-scheduled-quantities-events has successfully been returned", Type = typeof(ScheduledQuantities))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested operator-scheduled-quantities-events does not exist")]
        [SwaggerConsumes("application/json", "application/xml")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerOperation(Tags = new[] { "operator-scheduled-quantities-events" })]
        [Route("api/operator-scheduled-quantities-events/{id}")]
        public IHttpActionResult Get(int id)
        {
            var obj = _container.Resolve<IScheduledQuantityEventGet>().Invoke(id);

            if (obj != null)
            {
                return Ok(obj);
            }

            return NotFound();
        }

        /// <summary>
        /// Create a operator-scheduled-quantities-events
        /// </summary> 
        /// <param name="value">The operator-scheduled-quantities-events to be created</param>
        /// <remarks>Creates a operator-scheduled-quantities-events.  NOTE: This will only create data in the naesb_transaction_master/detail tables.</remarks>
        [SwaggerResponse(422, Description = "422(UnprocessableEntity)", Type = typeof(NaesbApiError))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "400(BadRequest)")]
        [SwaggerResponse(HttpStatusCode.Created, Description = "201(Created) - Returns the id of the created operator-scheduled-quantities-events", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok)", Type = null)]
        [SwaggerConsumes("application/json", "application/xml")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerOperation(Tags = new[] { "operator-scheduled-quantities-events" })]
        [Route("api/operator-scheduled-quantities-events")]
        public HttpResponseMessage Post(ScheduledQuantities value)
        {
            int id = _container.Resolve<IScheduledQuantityEventCreate>().Invoke(value);

            if (id > 0)
            {
                return Request.CreateResponse(HttpStatusCode.Created, id);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
        private bool ValidateKey(string[] key)
        {
            if (key?.Any() == true)
            {
                int number;
                if (key[2] != null && key[2].Length == 8 && int.TryParse(key[2], out number))
                {
                    return true;
                }
            }

            return false;
        }

        //// PUT: api/ScheduledQuantities/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ScheduledQuantities/5
        //public void Delete(int id)
        //{
        //}
    }
}
