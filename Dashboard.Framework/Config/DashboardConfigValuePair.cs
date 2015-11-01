using System.Xml.Serialization;


namespace NoeticTools.Dashboard.Framework.Config
{
    [XmlType("keyValue")]
    public class DashboardConfigValuePair
    {
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }

        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
    }
}