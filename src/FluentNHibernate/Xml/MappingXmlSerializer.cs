﻿using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Xml
{
    public class MappingXmlSerializer
    {
        public XmlDocument Serialize(HibernateMapping mapping)
        {
            return BuildXml(mapping);
            //return PerformSerialize(hbm);
        }

        public XmlDocument Serialize(HbmMapping hbm)
        {
            return PerformSerialize(hbm);
        }

        public XmlDocument SerializeHbmFragment(object hbm)
        {
            return PerformSerialize(hbm);
        }

        private static XmlDocument PerformSerialize(object hbm)
        {
            using (var stream = new MemoryStream())
            using (var writer = new XmlTextWriter(stream, System.Text.Encoding.Default))
            {
                var s = new XmlSerializer(hbm.GetType());
                s.Serialize(writer, hbm);
                stream.Position = 0;
                
                var doc = new XmlDocument();
                doc.Load(stream);

                using (var reader = new XmlTextReader("nhibernate-mapping.xsd"))
                    doc.Schemas.Add(XmlSchema.Read(reader, null));

                return doc;
            }
        }

        private static XmlDocument BuildXml(HibernateMapping rootMapping)
        {
            var xmlWriter = XmlWriterFactory.CreateHibernateMappingWriter();

            return xmlWriter.Write(rootMapping);
        }
    }
}
