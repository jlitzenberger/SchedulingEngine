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
using Nomination.BusinessLayer.Services.ScheduledQuantity.Naesb;
using Nomination.Domain.Naesb;
using Nomination.Persistence.Naesb;
using Nomination.Domain.ScheduledQuantities.Naesb;
using Nomination.Persistence.ScheduledQuantity;
using Nomination.Web.Api.Attributes;
using Swashbuckle.Swagger.Annotations;

namespace Nomination.Web.Api.Controllers
{
    [SwaggerControllerOrder(5)]
    public class NaesbScheduledQuantitiesEventController : ApiController
    {
        private static IContainer _container;
        public NaesbScheduledQuantitiesEventController()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => ServiceSettings.Get(
                    "xxxxx",
                    Nomination.Web.Api.Properties.Settings.Default.Environment
                )
            );
            builder.RegisterType<NaesbEventProcessRespository>().As<INaesbEventProcessRepository>();
            builder.RegisterType<NaesbEventProcessGet>().As<INaesbEventProcessGet>();
            builder.RegisterType<NaesbEventProcessUpdate>().As<INaesbEventProcessUpdate>();
            builder.RegisterType<NaesbEventProcessCreate>().As<INaesbEventProcessCreate>();

            builder.RegisterType<NaesbOperatorScheduledQuantitiesCreate>().As<INaesbOperatorScheduledQuantitiesCreate>();
            builder.RegisterType<NaesbOperatorScheduledQuantitiesGet>().As<INaesbOperatorScheduledQuantitiesGet>();

            builder.RegisterType<ScheduledQuantityEventRepository>().As<IScheduledQuantityEventRepository>();
            builder.RegisterType<ScheduledQuantityEventCreate>().As<IScheduledQuantityEventCreate>();
            builder.RegisterType<ScheduledQuantityEventProcess>().As<IScheduledQuantityEventProcess>();
            builder.RegisterType<ScheduledQuantityEventGet>().As<IScheduledQuantityEventGet>();

            builder.RegisterType<NaesbPipelineRepository>().As<INaesbPipelineRepository>();
            builder.RegisterType<NaesbPipelineGet>().As<INaesbPipelineGet>();

            builder.RegisterType<NaesbUtilityRepository>().As<INaesbUtilityRepository>();
            builder.RegisterType<NaesbUtilityGet>().As<INaesbUtilityGet>();

            builder.RegisterType<NaesbEventRepository>().As<INaesbEventRepository>();
            builder.RegisterType<NaesbEventGet>().As<INaesbEventGet>();
            builder.RegisterType<NaesbEventUpdate>().As<INaesbEventUpdate>();

            _container = builder.Build();
        }

        /// <summary>
        /// Get list of naesb-scheduled-quantities-events
        /// </summary> 
        /// <param name="pipeline_utility_gasday_cycle">Composite Id of the OSQ - {Pipeline}-{Utility}-{GasDay YYYYMMDD}-{Cycle}</param>
        /// <response code="200">naesb-operator-scheduled-quantitites-events returned OK</response>
        /// <remarks>Returns a list of naesb-operator-scheduled-quantitites-events.  NOTE: This will return the actual naesb .xml file data from the naesb_event_process table.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested naesb-operator-scheduled-quantities-events has successfully been returned", Type = typeof(IEnumerable<NaesbScheduledQuantities>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested naesb-operator-scheduled-quantities-events does not exist")]
        [SwaggerProduces("application/xml", "application/json")]
        [SwaggerOperation(Tags = new[] { "naesb-operator-scheduled-quantities-events" })]
        [Route("api/naesb-operator-scheduled-quantities-events")]
        public IHttpActionResult Get(string pipeline_utility_gasday_cycle)
        {
            string[] key = pipeline_utility_gasday_cycle.Split('-');

            if (ValidateKey(key) == true)
            {
                List<NaesbScheduledQuantities> nosqs = _container.Resolve<INaesbOperatorScheduledQuantitiesGet>()
                    .Invoke(
                        new DateTime(int.Parse(key[2].Substring(0, 4)), int.Parse(key[2].Substring(4, 2)), int.Parse(key[2].Substring(6, 2)))
                        , key[0]
                        , key[1]
                        , key[3]);

                if (nosqs.Count > 0)
                {
                    return Ok(nosqs);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get naesb-scheduled-quantities-events
        /// </summary> 
        /// <param name="id">Event Process Id of the OSQ</param>
        /// <response code="200">naesb-operator-scheduled-quantitites-events returned OK</response>
        /// <remarks>Returns a naesb-operator-scheduled-quantitites-events.  NOTE: This will return the actual naesb .xml file data from the naesb_event_process table.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested naesb-operator-scheduled-quantities-events has successfully been returned", Type = typeof(NaesbScheduledQuantities))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested naesb-operator-scheduled-quantities-events does not exist")]
        [SwaggerProduces("application/xml", "application/json")]
        [SwaggerOperation(Tags = new[] { "naesb-operator-scheduled-quantities-events" })]
        [Route("api/naesb-operator-scheduled-quantities-events/{id}")]
        public IHttpActionResult Get(int id)
        {
            NaesbScheduledQuantities obj = _container.Resolve<INaesbOperatorScheduledQuantitiesGet>().Invoke(id);

            if (obj != null)
            {
                return Ok(obj);
            }

            return NotFound();
        }

        /// <summary>
        /// Create a naesb-operator-scheduled-quantities-events.
        /// </summary> 
        /// <param name="value">The naesb-operator-scheduled-quantities-events to be created</param>
        /// <remarks>Create a naesb-operator-scheduled-quantitites-events.  NOTE: This only creates the naesb-operator-scheduled-quantities-events in Pegasys including the naesb_event_process table.</remarks>
        [SwaggerResponse(422, Description = "422(UnprocessableEntity)", Type = typeof(NaesbApiError))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "400(BadRequest)")]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "409(Conflict)")]
        [SwaggerResponse(HttpStatusCode.Created, Description = "201(Created) - Returns the id of the created object", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok)", Type = null)]
        [SwaggerConsumes("application/json", "application/xml")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerOperation(Tags = new[] { "naesb-operator-scheduled-quantities-events" })]
        [Route("api/naesb-operator-scheduled-quantities-events")]
        public IHttpActionResult Post(NaesbScheduledQuantities value)
        {
            try
            {
                int id = _container.Resolve<INaesbOperatorScheduledQuantitiesCreate>().Invoke(DateTime.Now, value);

                if (id > 0)
                {
                    string location = Request.RequestUri + "/" + id;
                    return Created(location, id);
                }
            }
            catch (NaesbError ex)
            {
                if (ex.ReasonCode == "101")
                {
                    return Conflict();
                }
            }
            return BadRequest();
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

        //// PUT: api/NaesbScheduledQuantities/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/NaesbScheduledQuantities/5
        //public void Delete(int id)
        //{
        //}

    }
}
