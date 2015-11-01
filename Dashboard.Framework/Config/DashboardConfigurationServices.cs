using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NoeticTools.Dashboard.Framework.Config;


namespace Dashboard.Config
{
    [XmlType("services")]
    public class DashboardConfigurationServices
    {
        public DashboardConfigurationServices()
        {
            Services = new DashboardServiceConfiguration[0];
        }

        [XmlArray]
        public DashboardServiceConfiguration[] Services { get; set; }

        public DashboardServiceConfiguration GetService(string serviceName)
        {
            var service =
                Services.SingleOrDefault(x => x.Name.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase));
            if (service != null)
            {
                return service;
            }
            service = new DashboardServiceConfiguration {Name = serviceName};
            Add(service);
            return service;
        }

        private void Add(DashboardServiceConfiguration service)
        {
            var list = new List<DashboardServiceConfiguration> {service};
            list.AddRange(Services);
            Services = list.ToArray();
        }
    }
}