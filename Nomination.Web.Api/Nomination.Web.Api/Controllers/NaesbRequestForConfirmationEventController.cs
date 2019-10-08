using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml;
using Autofac;
using Autofac.Core;
using CS.Common.Utilities;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.BusinessLayer.Services;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.BusinessLayer.Services.RequestForConfirmation;
using Nomination.BusinessLayer.Services.RequestForConfirmation.Event;
using Nomination.BusinessLayer.Services.RequestForConfirmation.Naesb;
using Nomination.Domain.RequestForConfirmation;
using Nomination.Domain.RequestForConfirmation.Naesb;
using Nomination.Persistence.Naesb;
using Nomination.Persistence.RequestForConfirmation;
using Nomination.Web.Api.Attributes;
using Swashbuckle.Swagger.Annotations;

namespace Nomination.Web.Api.Controllers
{
    [SwaggerControllerOrder(1)]
    public class NaesbRequestForConfirmationEventController : ApiController
    {
        private static IContainer _container;
        public NaesbRequestForConfirmationEventController()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => ServiceSettings.Get(
                    "xxxxx",
                    Nomination.Web.Api.Properties.Settings.Default.Environment
                )
            );
            builder.RegisterType<NaesbRepository>().As<INaesbRepository>();

            builder.RegisterType<NaesbEventProcessRespository>().As<INaesbEventProcessRepository>();
            builder.RegisterType<NaesbEventProcessGet>().As<INaesbEventProcessGet>();
            builder.RegisterType<NaesbEventProcessUpdate>().As<INaesbEventProcessUpdate>();
            builder.RegisterType<NaesbEventProcessCreate>().As<INaesbEventProcessCreate>();

            builder.RegisterType<NaesbEventRepository>().As<INaesbEventRepository>();
            builder.RegisterType<NaesbEventGet>().As<INaesbEventGet>();

            builder.RegisterType<NaesbPipelineRepository>().As<INaesbPipelineRepository>();
            builder.RegisterType<NaesbPipelineGet>().As<INaesbPipelineGet>();
            builder.RegisterType<NaesbPipelineGetByPipeline>().As<INaesbPipelineGetByPipeline>();

            builder.RegisterType<NaesbUtilityRepository>().As<INaesbUtilityRepository>();
            builder.RegisterType<NaesbUtilityGet>().As<INaesbUtilityGet>();
            builder.RegisterType<NaesbUtilityGetByUtility>().As<INaesbUtilityGetByUtility>();
            
            builder.RegisterType<NaesbRequestForConfirmationGet>().As<INaesbRequestForConfirmationGet>();
            builder.RegisterType<NaesbRequestForConfirmationHeaderGet>().As<INaesbRequestForConfirmationHeaderGet>();
            builder.RegisterType<NaesbRequestForConfirmationCreate>().As<INaesbRequestForConfirmationCreate>();
            builder.RegisterType<NaesbRequestForConfirmationGet>().As<INaesbRequestForConfirmationGet>();

            builder.RegisterType<RequestForConfirmationRepository>().As<IRequestForConfirmationRepository>();
            builder.RegisterType<RequestForConfirmationGet>().As<IRequestForConfirmationGet>();

            builder.RegisterType<RequestForConfirmationEventRepository>().As<IRequestForConfirmationEventRepository>();
            builder.RegisterType<RequestForConfirmationEventCreate>().As<IRequestForConfirmationEventCreate>();

            _container = builder.Build();
        }

        /// <summary>
        /// Get list of naesb-request-for-confirmation-events
        /// </summary> 
        /// <param name="pipeline_utility_gasday_cycle">Composite Id of the RFC - {Pipeline}-{Utility}-{GasDay YYYYMMDD}-{Cycle}</param>
        /// <response code="200">naesb-request-for-confirmation-events returned OK</response>
        /// <remarks>Returns a list of naesb-request-for-confirmation-events.  NOTE: This will return the actual naesb .xml file data from the naesb_event_process table.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested naesb-request-for-confirmation-events has successfully been returned", Type = typeof(IEnumerable<NaesbRequestForConfirmation>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested naesb-request-for-confirmation-events does not exist")]
        [SwaggerProduces("application/xml", "application/json")]
        [SwaggerOperation(Tags = new[] { "naesb-request-for-confirmation-events" })]
        [Route("api/naesb-request-for-confirmation-events")]
        public IHttpActionResult Get(string pipeline_utility_gasday_cycle)
        {
            string[] key = pipeline_utility_gasday_cycle.Split('-');

            if (ValidateKey(key) == true)
            {
                List<NaesbRequestForConfirmation> list = _container.Resolve<INaesbRequestForConfirmationGet>()
                    .Invoke(
                        new DateTime(int.Parse(key[2].Substring(0, 4)), int.Parse(key[2].Substring(4, 2)), int.Parse(key[2].Substring(6, 2)))
                        , key[0]
                        , key[1]
                        , key[3]);

                if (list.Count > 0)
                {
                    return Ok(list);
                }
            }

            return NotFound();
        }
        /// <summary>
        /// Get naesb-request-for-confirmation-events
        /// </summary> 
        /// <param name="id">Event Process Id of the RFC</param>
        /// <response code="200">naesb-request-for-confirmation-events returned OK</response>
        /// <remarks>Returns a list of naesb-request-for-confirmation-events.  NOTE: This will return the actual naesb .xml file data from the naesb_event_process table.</remarks>
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok) - The requested naesb-request-for-confirmation-events has successfully been returned", Type = typeof(NaesbRequestForConfirmation))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "404(Not Found) - The requested naesb-request-for-confirmation-events does not exist")]
        [SwaggerProduces("application/xml", "application/json")]
        [SwaggerOperation(Tags = new[] { "naesb-request-for-confirmation-events" })]
        [Route("api/naesb-request-for-confirmation-events/{id}")]
        public IHttpActionResult Get(int id)
        {
            NaesbRequestForConfirmation obj = _container.Resolve<INaesbRequestForConfirmationGet>().Invoke(id);

            if (obj != null)
            {
                return Ok(obj);
            }

            return NotFound();
        }

        /// <summary>
        /// Create a naesb-request-for-confirmation-events.
        /// </summary> 
        /// <param name="value">The naesb-request-for-confirmation-events to be created</param>
        /// <remarks>Create a naesb-request-for-confirmation.  NOTE: This only creates the naesb-confirmation-response-events in Pegasys but does not send it to the pipelines...YET</remarks>
        [SwaggerResponse(422, Description = "422(UnprocessableEntity)", Type = typeof(NaesbApiError))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "400(BadRequest)")]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "409(Conflict)")]
        [SwaggerResponse(HttpStatusCode.Created, Description = "201(Created) - Returns the id of the created object", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.OK, Description = "200(Ok)", Type = typeof(string))]
        [SwaggerConsumes("application/json", "application/xml")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerOperation(Tags = new[] { "naesb-request-for-confirmation-events" })]
        [Route("api/naesb-request-for-confirmation-events")]
        public IHttpActionResult Post(RequestForConfirmation value)
        {
            string naesbFileName = FormatNaesbRequestForConfirmationFileName(value);

            int id = _container.Resolve<INaesbRequestForConfirmationCreate>().Invoke(DateTime.Now, naesbFileName, value);

            //map domain rfc model to the Naesb rfc model.
            NaesbRequestForConfirmation nrfc = _container.Resolve<INaesbRequestForConfirmationGet>().Invoke(id);

            SaveNaesbRequestForConfirmationFile(nrfc, naesbFileName);

            if (id > 0)
            {
                string location = Request.RequestUri + "/" + id;
                return Created(location, id);
            }

            return BadRequest();
        }

        private static string FormatNaesbRequestForConfirmationFileName(RequestForConfirmation rfc)
        {
            string fileName = rfc.GasDay.ToString("yyyyMMdd") + "_" +
                              rfc.Cycle + "_" +
                              "RFC_" +
                              rfc.PartyIndentificaton.PipelineEntity + "_" +
                              rfc.PartyIndentificaton.UtilityEntity + ".xml";

            return fileName;
        }
        private static void SaveNaesbRequestForConfirmationFile(NaesbRequestForConfirmation nrfc, string ediFileName)
        {
            //serialize with out the namespace because GENTRAN can't handle it
            XmlDocument xmlFile = XmlTransformer.XmlSerialize(nrfc, true);
            //xmlFile.Save(Properties.Settings.Default.NaesbOutboundUnc + ediFileName);
            xmlFile.Save(Nomination.Web.Api.Properties.Settings.Default.NaesbOutboundFileUnc + ediFileName);
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



        //// POST: api/NaesbRequestForConfirmation
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/NaesbRequestForConfirmation/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/NaesbRequestForConfirmation/5
        //public void Delete(int id)
        //{
        //}
    }
}
