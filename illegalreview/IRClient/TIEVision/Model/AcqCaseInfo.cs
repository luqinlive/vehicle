/* 
 Licensed under the Apache License, Version 2.0
    
 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace TIEVision.Model
{
    [XmlRoot(ElementName = "ext")]
    public class Ext
    {
        [XmlAttribute(AttributeName = "channal")]
        public string Channal { get; set; }
        [XmlAttribute(AttributeName = "streamcode")]
        public string Streamcode { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "framerate")]
        public string Framerate { get; set; }
        [XmlAttribute(AttributeName = "pixtype")]
        public string Pixtype { get; set; }
        [XmlAttribute(AttributeName = "codectype")]
        public string Codectype { get; set; }
        [XmlAttribute(AttributeName = "streamtype")]
        public string Streamtype { get; set; }
    }

    [XmlRoot(ElementName = "conn")]
    public class Conn
    {
        [XmlAttribute(AttributeName = "ipaddr")]
        public string Ipaddr { get; set; }
        [XmlAttribute(AttributeName = "port")]
        public string Port { get; set; }
        [XmlAttribute(AttributeName = "user")]
        public string User { get; set; }
        [XmlAttribute(AttributeName = "password")]
        public string Password { get; set; }
    }

    [XmlRoot(ElementName = "device")]
    public class Device
    {
        [XmlElement(ElementName = "conn")]
        public Conn Conn { get; set; }
        [XmlElement(ElementName = "ext")]
        public Ext Ext { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "parentid")]
        public string ParentId { get; set; }
        [XmlAttribute(AttributeName = "protocolid")]
        public string Protocolid { get; set; }
        [XmlAttribute(AttributeName = "num")]
        public string Num { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
        [XmlAttribute(AttributeName = "Log")]
        public string Log { get; set; }
        [XmlAttribute(AttributeName = "Lat")]
        public string Lat { get; set; }
        [XmlAttribute(AttributeName = "state")]
        public string State { get; set; }
        [XmlAttribute(AttributeName = "calc")]
        public string Calc { get; set; }
    }

    [XmlRoot(ElementName = "avfile")]
    public class Avfile
    {
        [XmlElement(ElementName = "ext")]
        public Ext Ext { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "parentid")]
        public string Parentid { get; set; }
        [XmlAttribute(AttributeName = "caseid")]
        public string Caseid { get; set; }
        [XmlAttribute(AttributeName = "protocolid")]
        public string Protocolid { get; set; }
        [XmlAttribute(AttributeName = "author")]
        public string Author { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "filename")]
        public string Filename { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
        [XmlAttribute(AttributeName = "filetype")]
        public string Filetype { get; set; }
        [XmlAttribute(AttributeName = "duration")]
        public string Duration { get; set; }
        [XmlAttribute(AttributeName = "filesize")]
        public string Filesize { get; set; }
        [XmlAttribute(AttributeName = "calc")]
        public string Calc { get; set; }
        [XmlAttribute(AttributeName = "recordtime")]
        public string Recordtime { get; set; }
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }
    }

    [XmlRoot(ElementName = "picture")]
    public class Picture
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "parentid")]
        public string Parentid { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "filesize")]
        public string Filesize { get; set; }
        [XmlAttribute(AttributeName = "facesize")]
        public string Facesize { get; set; }
        [XmlAttribute(AttributeName = "analysis")]
        public string Analysis { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
    }

    [XmlRoot(ElementName = "case")]
    public class Case
    {
        
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "parentid")]
        public string Parentid { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
        [XmlAttribute(AttributeName = "nextindex")]
        public string NextIndex { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "group")]
        public List<Group> Groups { get; set; }
        [XmlElement(ElementName = "avfile")]
        public List<Avfile> Avfiles { get; set; }
        [XmlElement(ElementName = "device")]
        public List<Device> Devices { get; set; }

        [XmlElement(ElementName = "picture")]
        public List<Picture> Pictures { get; set; }

    }

   

    [XmlRoot(ElementName = "group")]
    public class Group
    {

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "parentid")]
        public string Parentid { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "avfile")]
        public List<Avfile> Avfiles { get; set; }
        [XmlElement(ElementName = "device")]
        public List<Device> Devices { get; set; }
        [XmlElement(ElementName = "group")]
        public List<Group> Groups { get; set; }
    }

    [XmlRoot(ElementName = "caseinfo")]
    public class Caseinfo
    {
        [XmlElement(ElementName = "case")]
        public List<Case> Case { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "nextindex")]
        public string NextIndex { get; set; }
    }

}