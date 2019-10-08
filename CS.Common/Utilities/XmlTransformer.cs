using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CS.Common.Utilities
{
    public static class XmlTransformer
    {
        public static XmlDocument XmlSerialize<T>(T obj, bool removeNamespace = false)
        {
            if (obj != null)
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    if (removeNamespace)
                    {
                        var ns = new XmlSerializerNamespaces();
                        ns.Add("", "");
                        serializer.Serialize(stream, obj, ns);
                    }
                    else
                    {
                        serializer.Serialize(stream, obj);
                    }
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                }

                return xmlDocument;
            }

            return null;
        }
        public static T XmlDeserialize<T>(string xml)
        {
            if (xml != null)
            {
                T obj;
                using (TextReader sr = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    obj = (T)serializer.Deserialize(sr);
                }

                return obj;
            }

            return default(T);
        }
        public static T XmlDeserialize<T>(XmlDocument xml)
        {
            if (xml != null)
            {
                T obj;
                using (TextReader sr = new StringReader(xml.InnerXml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    obj = (T)serializer.Deserialize(sr);
                }

                return obj;
            }

            return default(T);
        }
        public static XmlDocument ConvertToXmlDocument(FileInfo file, bool removeEncoding = false)
        {
            if (file != null)
            {
                XmlDocument obj = new XmlDocument();
                obj.Load(file.FullName);

                if (removeEncoding)
                {
                    foreach (XmlNode node in obj)
                    {
                        if (node.NodeType == XmlNodeType.XmlDeclaration)
                        {
                            obj.RemoveChild(node);

                            XmlDeclaration xmlDeclaration = obj.CreateXmlDeclaration("1.0", string.Empty, null);
                            XmlElement root = obj.DocumentElement;
                            obj.InsertBefore(xmlDeclaration, root);
                        }
                    }
                }

                return obj;
            }

            return null;
        }
    }
}
