using System.Xml.Serialization;


namespace NoeticTools.SystemsDashboard.Framework.Config
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