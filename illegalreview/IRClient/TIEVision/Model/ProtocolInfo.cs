
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
//using TIEVision.Model;
namespace TIEVision.Model
{


    [XmlRoot(ElementName = "factory")]
    public class Factory
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "engname")]
        public string Engname { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "protocol")]
        public List<Protocol> Protocol { get; set; }
    }


    [XmlRoot(ElementName = "protocolinfo")]
    public class ProtocolInfo
    {
        [XmlElement(ElementName = "factory")]
        public List<Factory> Factory { get; set; }

    }

    [XmlRoot(ElementName = "protocol")]
    public class Protocol
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }

        [XmlAttribute(AttributeName = "sdkversion")]
        public string Sdkversion { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "filename")]
        public string Filename { get; set; }

        [XmlElement(ElementName = "conn")]
        public Conn Conn { get; set; }

    }
}

//[XmlRoot(ElementName = "protocol")]
//public class Conn
//{

//}





