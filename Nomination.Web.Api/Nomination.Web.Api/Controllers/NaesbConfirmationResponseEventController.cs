using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using Autofac;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services;
using Nomination.BusinessLayer.Services.ConfirmationResponse.Event;
using Nomination.BusinessLayer.Services.ConfirmationResponse.Naesb;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.Domain.ConfirmationResponse.Naesb;
using Nomination.Domain.Naesb;
using Nomination.Persistence.ConfirmationResponse;
using Nomination.Persistence.Naesb;
using Nomination.Web.Api.Attributes;
using Swashbuckle.Swagger.Annotations;

namespace Nomination.Web.Api.Controllers
{
    public class NaesbApiError
    {
        public string ReasonCode { get; set; }
        public string Value { get; set; }
    }

    [SwaggerControllerOrder(3)]
    public class NaesbConfirmationResponseEventController : ApiController
    {
        private static IContainer _container;
        public NaesbConfirmationResponseEventController()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => ServiceSettings.Get(
                     "xxxxx",
                     Nomination.Web.Api.Properties.Settings.Default.Environment
                 )
            );
            builder.RegisterType<NaesbEventProcessRespository>().As<INaesbEventProcessRepository>();
            builder.RegisterType<NaesbConfirmationResponseCreate>().As<INaesbConfirmationResponseCreate>();
            builder.RegisterType<NaesbConfirmationResponseGet>().As<INaesbConfirmationResponseGet>();

            builder.RegisterType<ConfirmationResponseEventRepository>().As<IConfirmationResponseEventRepository>();
            builder.RegisterType<ConfirmationResponseEventCreate>().As<IConfirmationResponseEventCreate>();
            builder.RegisterType<ConfirmationResponseEventProcess>().As<IConfirmationResponseEventProcess>();
            builder.RegisterType<ConfirmationResponseEventGet>().As<IConfirmationResponseEventGet>();

            builder.RegisterType<NaesbPipelineRepository>().As<INaesbPipelineRepository>();
            builder.RegisterType<NaesbPipelineGet>().As<INaesbPipelineGet>();

            builder.RegisterType<NaesbUtilityRepository>().As<INaesbUtilityRepository>();
            builder.RegisterType<NaesbUtilityGet>().As<INaesbUtilityGet>();

            builder.RegisterType<NaesbEventProcessRespository>().As<INaesbEventProcessRepository>();
            builder.RegisterType<NaesbEventProcessGet>().As<INaesbEventProcessGet>();
            builder.RegisterType<NaesbEventProcessCreate>().As<INaesbEventProcessCreate>();
            builder.RegisterType<NaesbEventProcessUpdate>().As<INaesbEventProcessUpdate>();

            builder.RegisterType<NaesbEventRepository>().As<INaesbEventRepository>();
            builder.RegisterType<NaesbEventGet>().As<INaesbEventGet>();
            builder.RegisterType<NaesbEventUpdate>().As<INaesbEventUpdate>();

            _container = builder.Build();
        }

        /// <summary>
        /// Get list of naesb-confirmation-response-events
        /// </summary> 
        /// <param name="pipeline_utility_gasday_cycle">Composite Id of the CR - {Pipeline}-{Utility}-{GasDay YYYYMMDD}-{Cycle}</param>
        /// <response code="200">naesb-confirmation-response-events returned OK</response>
        /// <remarks>Returns a list of naesb-confirmation-response-events.  NOTE: This will return the actual naesb .xml file data from the naesb_event_process table.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested naesb-confirmation-response-events has successfully been returned", Type = typeof(IEnumerable<NaesbConfirmationResponse>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested naesb-confirmation-response-events does not exist")]
        [SwaggerProduces("application/xml", "application/json")]
        [SwaggerOperation(Tags = new[] { "naesb-confirmation-response-events" })]
        [Route("api/naesb-confirmation-response-events")]
        public IHttpActionResult Get(string pipeline_utility_gasday_cycle)
        {
            string[] key = pipeline_utility_gasday_cycle.Split('-');

            if (ValidateKey(key) == true)
            {
                List<NaesbConfirmationResponse> ncrs = _container.Resolve<INaesbConfirmationResponseGet>()
                    .Invoke(
                          new DateTime(int.Parse(key[2].Substring(0, 4)), int.Parse(key[2].Substring(4, 2)), int.Parse(key[2].Substring(6, 2)))
                        , key[0]
                        , key[1]
                        , key[3]);

                if (ncrs.Count > 0)
                {
                    return Ok(ncrs);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get naesb-confirmation-response-events
        /// </summary> 
        /// <param name="id">Event Process Id of the CR</param>
        /// <response code="200">naesb-confirmation-response-events returned OK</response>
        /// <remarks>Returns a naesb-confirmation-response-events.  NOTE: This will return the actual naesb .xml file data from the naesb_event_process table.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested naesb-confirmation-response-events has successfully been returned", Type = typeof(NaesbConfirmationResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested naesb-confirmation-response-events does not exist")]
        [SwaggerProduces("application/xml", "application/json")]
        [SwaggerOperation(Tags = new[] { "naesb-confirmation-response-events" })]
        [Route("api/naesb-confirmation-response-events/{id}")]
        public IHttpActionResult Get(int id)
        {
            NaesbConfirmationResponse obj = _container.Resolve<INaesbConfirmationResponseGet>().Invoke(id);

            if (obj != null)
            {
                return Ok(obj);
            }

            return NotFound();
        }

        /// <summary>
        /// Create a naesb-confirmation-response-events.
        /// </summary> 
        /// <param name="value">The naesb-confirmation-response-events to be created</param>
        /// <remarks>Create a naesb-confirmation-response.  NOTE: This only creates the naesb-confirmation-response-events in Pegasys including the naesb_event_process table.</remarks>
        [SwaggerResponse(422, Description = "422(UnprocessableEntity)", Type = typeof(NaesbApiError))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "400(BadRequest)")]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "409(Conflict)")]
        [SwaggerResponse(HttpStatusCode.Created, Description = "201(Created) - Returns the id of the created object", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok)",  Type = typeof(string))]
        [SwaggerConsumes("application/json", "application/xml")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerOperation(Tags = new[] { "naesb-confirmation-response-events" })]
        [Route("api/naesb-confirmation-response-events")]
        public IHttpActionResult Post(NaesbConfirmationResponse value)
        {
            try
            {
                int id = _container.Resolve<INaesbConfirmationResponseCreate>().Invoke(DateTime.Now, value);

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

        //// PUT: api/NaesbConfirmationResponse/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/NaesbConfirmationResponse/5
        //public void Delete(int id)
        //{
        //}
    }
}
