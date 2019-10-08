using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autofac;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services;
using Nomination.BusinessLayer.Services.ConfirmationResponse;
using Nomination.BusinessLayer.Services.ConfirmationResponse.Event;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.Domain.ConfirmationResponse;
using Nomination.Domain.Naesb;
using Nomination.Persistence.ConfirmationResponse;
using Nomination.Persistence.Naesb;
using Nomination.Web.Api.Attributes;
using Swashbuckle.Swagger.Annotations;

namespace Nomination.Web.Api.Controllers
{
    [SwaggerControllerOrder(4)]
    public class ConfirmationResponseEventController : ApiController
    {
        private static IContainer _container;
        public ConfirmationResponseEventController()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => ServiceSettings.Get(
                    "xxxxx",
                    Nomination.Web.Api.Properties.Settings.Default.Environment
                )
            );
            builder.RegisterType<ConfirmationResponseEventRepository>().As<IConfirmationResponseEventRepository>();
            builder.RegisterType<ConfirmationResponseEventGet>().As<IConfirmationResponseEventGet>();
            builder.RegisterType<ConfirmationResponseEventCreate>().As<IConfirmationResponseEventCreate>();
            builder.RegisterType<ConfirmationResponseEventProcess>().As<IConfirmationResponseEventProcess>();

            builder.RegisterType<NaesbEventRepository>().As<INaesbEventRepository>();
            builder.RegisterType<NaesbEventGet>().As<INaesbEventGet>();

            builder.RegisterType<NaesbPipelineRepository>().As<INaesbPipelineRepository>();
            builder.RegisterType<NaesbPipelineGet>().As<INaesbPipelineGet>();

            builder.RegisterType<NaesbUtilityRepository>().As<INaesbUtilityRepository>();
            builder.RegisterType<NaesbUtilityGet>().As<INaesbUtilityGet>();

            _container = builder.Build();
        }

        /// <summary>
        /// Get a list of confirmation-response-events
        /// </summary> 
        /// <param name="pipeline_utility_gasday_cycle">Composite Id of the CR - {Pipeline}-{Utility}-{GasDay YYYYMMDD}-{Cycle}</param>
        /// <response code="200">confirmation-response-events returned OK</response>
        /// <remarks>Returns a list of confirmation-response-events.  NOTE: This will return data from the naesb_transaction_master/detail tables.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested confirmation-response-events has successfully been returned", Type = typeof(IEnumerable<ConfirmationResponse>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested confirmation-response-events does not exist")]
        [SwaggerProduces("application/xml", "application/json")]
        [SwaggerOperation(Tags = new[] { "confirmation-response-events" })]
        [Route("api/confirmation-response-events")]
        public IHttpActionResult Get(string pipeline_utility_gasday_cycle)
        {
            string[] key = pipeline_utility_gasday_cycle.Split('-');

            if (ValidateKey(key) == true)
            {
                var cr = _container.Resolve<IConfirmationResponseEventGet>()
                    .Invoke(
                          key[0]
                        , key[1]
                        , new DateTime(int.Parse(key[2].Substring(0, 4)), int.Parse(key[2].Substring(4, 2)), int.Parse(key[2].Substring(6, 2)))
                        , key[3]);

                if (cr != null && cr.Count > 0)
                {
                    return Ok(cr);
                }
            }

            return NotFound();
        }


        /// <summary>
        /// Get confirmation-response-events
        /// </summary> 
        /// <param name="id">Naesb transaction master Id of the CR</param>
        /// <response code="200">confirmation-response-events returned OK</response>
        /// <remarks>Returns a confirmation-response-events.  NOTE: This will return data from the naesb_transaction_master/detail tables.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested confirmation-response-events has successfully been returned", Type = typeof(ConfirmationResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested confirmation-response-events does not exist")]
        [SwaggerConsumes("application/json", "application/xml")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerOperation(Tags = new[] { "confirmation-response-events" })]
        [Route("api/confirmation-response-events/{id}")]
        public IHttpActionResult Get(int id)
        {
            var cr = _container.Resolve<IConfirmationResponseEventGet>().Invoke(id);

            if (cr != null)
            {
                return Ok(cr);
            }

            return NotFound();
        }

        /// <summary>
        /// Create a confirmation-response-events
        /// </summary> 
        /// <param name="value">The confirmation-response-events to be created</param>
        /// <remarks>Creates a confirmation-response-events.  NOTE: This will only create data in the naesb_transaction_master/detail tables.</remarks>
        [SwaggerResponse(422, Description = "422(UnprocessableEntity)", Type = typeof(NaesbApiError))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "400(BadRequest)")]
        [SwaggerResponse(HttpStatusCode.Created, Description = "201(Created) - Returns the id of the created confirmation-response-events", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok)", Type = null)]
        [SwaggerConsumes("application/json", "application/xml")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerOperation(Tags = new[] { "confirmation-response-events" })]
        [Route("api/confirmation-response-events")]
        public HttpResponseMessage Post(ConfirmationResponse value)
        {
            int id = _container.Resolve<IConfirmationResponseEventCreate>().Invoke(value);

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

        //// PUT: api/ConfirmationResponse/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ConfirmationResponse/5
        //public void Delete(int id)
        //{
        //}
    }
}
