using System.Xml.Serialization;
using Dashboard.Config;

namespace NoeticTools.Dashboard.Framework.Config
{
    [XmlRoot("Dashboards", Namespace = "http://www.cpandl.com", IsNullable = false)]
    [XmlType("configurations")]
    public class DashboardConfigurations
    {
        public DashboardConfigurations()
        {
            Current = string.Empty;
            Configurations = new DashboardConfiguration[0];
            Services = new DashboardConfigurationServices();
        }

        [XmlAttribute(AttributeName = "current")]
        public string Current { get; set; }

        [XmlArray(ElementName = "configurations")]
        public DashboardConfiguration[] Configurations { get; set; }

        [XmlElement(ElementName = "services")]
        public DashboardConfigurationServices Services { get; set; }
    }
}