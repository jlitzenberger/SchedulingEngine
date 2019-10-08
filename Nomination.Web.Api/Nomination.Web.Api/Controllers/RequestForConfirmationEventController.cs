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
using Nomination.BusinessLayer.Services.RequestForConfirmation;
using Nomination.BusinessLayer.Services.RequestForConfirmation.Event;
using Nomination.Domain.RequestForConfirmation;
using Nomination.Persistence.Naesb;
using Nomination.Persistence.RequestForConfirmation;
using Nomination.Web.Api.Attributes;
using Swashbuckle.Swagger.Annotations;

namespace Nomination.Web.Api.Controllers
{
    [SwaggerControllerOrder(2)]
    public class RequestForConfirmationEventController : ApiController
    {
        private static IContainer _container;
        public RequestForConfirmationEventController()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => ServiceSettings.Get(
                    "xxxxx",
                    Nomination.Web.Api.Properties.Settings.Default.Environment
                )
            );
            builder.RegisterType<RequestForConfirmationEventRepository>().As<IRequestForConfirmationEventRepository>();
            builder.RegisterType<RequestForConfirmationEventGet>().As<IRequestForConfirmationEventGet>();

            builder.RegisterType<RequestForConfirmationEventRepository>().As<IRequestForConfirmationEventRepository>();
            builder.RegisterType<RequestForConfirmationEventCreate>().As<IRequestForConfirmationEventCreate>();

            _container = builder.Build();
        }

        /// <summary>
        /// Get a list of request-for-confirmation-events
        /// </summary> 
        /// <param name="pipeline_utility_gasday_cycle">Composite Id of the RFC - {Pipeline}-{Utility}-{GasDay YYYYMMDD}-{Cycle}</param>
        /// <response code="200">List of request-for-confirmation-events returned OK</response>
        /// <remarks>Returns a list of request-for-confirmation-events.  NOTE: This will return data from the naesb_transaction_master/detail tables.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested request-for-confirmation-events has successfully been returned", Type = typeof(List<RequestForConfirmation>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested request-for-confirmation-events does not exist")]
        [SwaggerConsumes("application/json", "application/xml")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerOperation(Tags = new[] { "request-for-confirmation-events" })]
        [Route("api/request-for-confirmation-events")]
        public IHttpActionResult Get(string pipeline_utility_gasday_cycle)
        {
            string[] key = pipeline_utility_gasday_cycle.Split('-');

            if (ValidateKey(key) == true)
            {
                List<RequestForConfirmation> objs = _container.Resolve<IRequestForConfirmationEventGet>().Invoke(
                      key[0]
                    , key[1]
                    , new DateTime(int.Parse(key[2].Substring(0, 4)), int.Parse(key[2].Substring(4, 2)), int.Parse(key[2].Substring(6, 2)))
                    , key[3]
                );

                if (objs.Count > 0)
                {
                    return Ok(objs);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get a request-for-confirmation-events
        /// </summary> 
        /// <param name="id">Naesb transaction master Id of the RFC</param>
        /// <response code="200">request-for-confirmation-events returned OK</response>
        /// <remarks>Returns a list of request-for-confirmation-events.  NOTE: This will return data from the naesb_transaction_master/detail tables.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested request-for-confirmation-events has successfully been returned", Type = typeof(RequestForConfirmation))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested request-for-confirmation-events does not exist")]
        [SwaggerConsumes("application/json", "application/xml")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerOperation(Tags = new[] { "request-for-confirmation-events" })]
        [Route("api/request-for-confirmation-events/{id}")]
        public IHttpActionResult Get(int id)
        {
            RequestForConfirmation obj = _container.Resolve<IRequestForConfirmationEventGet>().Invoke(id);

            if (obj != null)
            {
                return Ok(obj);
            }

            return NotFound();
        }

        /// <summary>
        /// Create a request-for-confirmation-events
        /// </summary> 
        /// <param name="value">The request-for-confirmation-events to be created</param>
        /// <remarks>Creates a request-for-confirmation-events.  NOTE: This will only create data in the naesb_transaction_master/detail tables.</remarks>
        [ValidationActionFilter]
        [SwaggerResponse(422, Description = "422(UnprocessableEntity)", Type = typeof(NaesbApiError))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "400(BadRequest)")]
        [SwaggerResponse(HttpStatusCode.Created, Description = "201(Created) - Returns the id of the created request-for-confirmation-events", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok)", Type = null)]
        [SwaggerConsumes("application/json", "application/xml")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerOperation(Tags = new[] { "request-for-confirmation-events" })]
        [Route("api/request-for-confirmation-events")]
        public HttpResponseMessage Post(RequestForConfirmation value)
        {
            int id = _container.Resolve<IRequestForConfirmationEventCreate>().Invoke(value);

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

        //// POST: api/RequestForConfirmationEvent
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/RequestForConfirmationEvent/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/RequestForConfirmationEvent/5
        //public void Delete(int id)
        //{
        //}
    }
}
