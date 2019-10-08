using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autofac;
using Swashbuckle.Swagger.Annotations;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.BusinessLayer.Services.RequestForConfirmation;
using Nomination.Domain.RequestForConfirmation;
using Nomination.Persistence.Naesb;
using Nomination.Persistence.RequestForConfirmation;
using Nomination.Web.Api.Attributes;

namespace Nomination.Web.Api.Controllers
{
    [SwaggerControllerOrder(0)]
    public class RequestForConfirmationController : ApiController
    {
        private static IContainer _container;
        public RequestForConfirmationController()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<RequestForConfirmationRepository>().As<IRequestForConfirmationRepository>();
            builder.RegisterType<RequestForConfirmationGet>().As<IRequestForConfirmationGet>();

            builder.RegisterType<NaesbPipelineRepository>().As<INaesbPipelineRepository>();
            builder.RegisterType<NaesbPipelineGet>().As<INaesbPipelineGet>();

            builder.RegisterType<NaesbUtilityRepository>().As<INaesbUtilityRepository>();
            builder.RegisterType<NaesbUtilityGet>().As<INaesbUtilityGet>();

            _container = builder.Build();
        }

        /// <summary>
        /// Get a request-for-confirmations
        /// </summary> 
        /// <param name="pipeline_utility_gasday_cycle">Composite Id of the RFC - {Pipeline}-{Utility}-{GasDay YYYYMMDD}-{Cycle}</param>
        /// <response code="200">request-for-confirmations returned OK</response>
        /// <remarks>Note: This is coming from the Pegasys database through a stored proc...not the naesb transaction tables...and should only return 1.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested request-for-confirmations has successfully been returned", Type = typeof(RequestForConfirmation))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested request-for-confirmations does not exist")]
        [SwaggerConsumes("application/json", "application/xml")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerOperation(Tags = new[] { "request-for-confirmations" })]
        [Route("api/request-for-confirmations/{pipeline_utility_gasday_cycle}")]
        public IHttpActionResult Get(string pipeline_utility_gasday_cycle)
        {
            string[] key = pipeline_utility_gasday_cycle.Split('-');

            if (ValidateKey(key) == true)
            {
                RequestForConfirmation obj = _container.Resolve<IRequestForConfirmationGet>().Invoke(
                      key[0]
                    , key[1]
                    , new DateTime(int.Parse(key[2].Substring(0, 4)), int.Parse(key[2].Substring(4, 2)), int.Parse(key[2].Substring(6, 2)))
                    , key[3]
                );
                
                if (obj != null)
                {
                    return Ok(obj);
                }
            }
            
            return NotFound();
        }
        ///// <summary>
        ///// Get request-for-confirmation
        ///// </summary> 
        ///// <param name="id">Naesb transaction master Id of the RFC</param>
        ///// <response code="200">request-for-confirmation returned OK</response>
        ///// <remarks>Note: This is coming from the Pegasys database through a stored proc...not the naesb transaction tables...and should only return 1.</remarks>
        //[SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested request-for-confirmation has successfully been returned", Type = typeof(RequestForConfirmation))]
        //[SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested request-for-confirmation does not exist")]
        //[SwaggerConsumes("application/json", "application/xml")]
        //[SwaggerProduces("application/json", "application/xml")]
        //[SwaggerOperation(Tags = new[] { "request-for-confirmation" })]
        //[Route("api/request-for-confirmation/{id}")]
        //public IHttpActionResult Get(int id)
        //{
        //    var obj = _container.Resolve<IRequestForConfirmationGet>().Invoke(id);

        //    if (obj != null)
        //    {
        //        return Ok(obj);
        //    }

        //    return NotFound();
        //}
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
    }
}



