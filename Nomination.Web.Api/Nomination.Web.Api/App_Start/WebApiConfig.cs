using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Nomination.Domain.ConfirmationResponse;
using Nomination.Domain.ConfirmationResponse.Naesb;
using Nomination.Domain.RequestForConfirmation;
using Nomination.Domain.RequestForConfirmation.Naesb;
using Nomination.Domain.ScheduledQuantities;
using Nomination.Domain.ScheduledQuantities.Naesb;
using Stream = System.IO.Stream;

namespace Nomination.Web.Api
{
    public class SectionMediaTypeFormatter<T> : BufferedMediaTypeFormatter
    {
        public SectionMediaTypeFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
        }

        public override bool CanReadType(Type type)
        {
            return typeof(T) == type;
        }
        public override bool CanWriteType(Type type)
        {
            return typeof(T) == type;
        }
        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            var xmlWriterSettings = new XmlWriterSettings { OmitXmlDeclaration = true, Encoding = Encoding.UTF8 };

            using (XmlWriter writer = XmlWriter.Create(writeStream, xmlWriterSettings))
            {
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                var serializer = new XmlSerializer(type);
                serializer.Serialize(writer, value, namespaces);
            }
        }
        public override object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var serializer = new XmlSerializer(type);
            return serializer.Deserialize(readStream);
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json")); //Used for Patch

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            if (jsonFormatter != null)
            {
                jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); //Converts all responses in camel case format
                jsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc; //Converts all responses in date time UTC format
                jsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter()); //Converts all enums to string
            }
            
            var xml = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.UseXmlSerializer = true;

            //https://stackoverflow.com/questions/21388854/custom-xmlserializer-only-adds-namespaces-in-webapi
            GlobalConfiguration.Configuration.Formatters.Insert(0, new SectionMediaTypeFormatter<NaesbRequestForConfirmation>());
            GlobalConfiguration.Configuration.Formatters.Insert(1, new SectionMediaTypeFormatter<RequestForConfirmation>());

            GlobalConfiguration.Configuration.Formatters.Insert(2, new SectionMediaTypeFormatter<NaesbScheduledQuantities>());
            GlobalConfiguration.Configuration.Formatters.Insert(3, new SectionMediaTypeFormatter<List<NaesbScheduledQuantities>>());
            GlobalConfiguration.Configuration.Formatters.Insert(4, new SectionMediaTypeFormatter<ScheduledQuantities>());
            GlobalConfiguration.Configuration.Formatters.Insert(4, new SectionMediaTypeFormatter<List<ScheduledQuantities>>());

            GlobalConfiguration.Configuration.Formatters.Insert(5, new SectionMediaTypeFormatter<NaesbConfirmationResponse>());
            GlobalConfiguration.Configuration.Formatters.Insert(6, new SectionMediaTypeFormatter<List<NaesbConfirmationResponse>>());
            GlobalConfiguration.Configuration.Formatters.Insert(7, new SectionMediaTypeFormatter<ConfirmationResponse>());
            GlobalConfiguration.Configuration.Formatters.Insert(8, new SectionMediaTypeFormatter<List<ConfirmationResponse>>());



            //var settings = new DataContractSerializerSettings();
            //var xmlDictionary = new XmlDictionary();
            //settings.RootName = xmlDictionary.Add("MyRootName");
            //settings.RootNamespace = xmlDictionary.Add("MyNamespace");

            //var dcs = new DataContractSerializer(typeof(NaesbRequestForConfirmation), settings);
            //xml.SetSerializer<NaesbRequestForConfirmation>(dcs);


            //xml.SetSerializer<NaesbRequestForConfirmation>(
            //    new XmlSerializer(typeof(NaesbRequestForConfirmation),new XmlRootAttribute() { ElementName = "hh", Namespace="" })
            //);



            //XmlElementAttribute attr = new XmlElementAttribute();
            //attr.ElementName = "xmlns:xsd";

            //XmlAttributes attrs = new XmlAttributes();
            //attrs.XmlElements.Add(attr);

            //XmlRootAttribute xRoot = new XmlRootAttribute();
            //// Set a new Namespace and ElementName for the root element.
            //xRoot.ElementName = "RequestForConfirmation";
            //xRoot.Namespace = "http://www.cpandl.com";

            //attrs.XmlRoot = new XmlRootAttribute
            //{
            //    ElementName = "RequestForConfirmation",
            //    Namespace = "http://www.cpandl.com"
            //};




            //XmlAttributes ignore = new XmlAttributes()
            //{
            //    XmlIgnore = true
            //};

            //XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            //overrides.Add(typeof(XmlRootAttribute), "xmlns", ignore);

            //xml.SetSerializer<NaesbRequestForConfirmation>(
            //    new XmlSerializer(typeof(NaesbRequestForConfirmation), overrides)
            //);






            ////WORKS..not namespace though
            //xml.SetSerializer<NaesbRequestForConfirmation>(
            //    new XmlSerializer(typeof(NaesbRequestForConfirmation), new XmlRootAttribute { ElementName = "pp", IsNullable = true, Namespace = "xmlns:xsd", DataType = "" })
            //);




            //xml.SetSerializer<NaesbScheduledQuantities>(
            //    new XmlSerializer(typeof(NaesbScheduledQuantities), new XmlRootAttribute { ElementName = "hh", Namespace = "" })
            //);



            //XmlSerializer serializer = new XmlSerializer(obj.GetType());
            //using (MemoryStream stream = new MemoryStream())
            //{

            //        var ns = new XmlSerializerNamespaces();
            //        ns.Add("", "");
            //        serializer.Serialize(stream, obj, ns);

            //    stream.Position = 0;
            //    xmlDocument.Load(stream);
            //}

            //MemoryStream stream = new MemoryStream()
            //{
            //    Position = 0
            //};
            //var namespaces = new XmlSerializerNamespaces();
            //namespaces.Add("", "");

            //xml.CreateXmlSerializer(typeof(NaesbRequestForConfirmation)).Serialize(stream, null, namespaces);











            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
