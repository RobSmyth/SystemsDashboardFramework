using System.Xml.Serialization;


namespace NoeticTools.SystemsDashboard.Framework.Config
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