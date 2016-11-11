using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NoeticTools.TeamStatusBoard.Framework.Config;


namespace NoeticTools.TeamStatusBoard.Framework.Persistence.Xml
{
    [XmlType("service")]
    public class DashboardServiceConfiguration : ItemConfigurationBase
    {
        private const string DefaultGuid = "B714E5AB-8F0D-4C4A-A6CB-44B6C3AEEE88";

        public DashboardServiceConfiguration()
        {
            TypeId = Guid.Parse(DefaultGuid);
            Name = string.Empty;
            Values = new DashboardConfigValuePair[0];
        }

        [XmlIgnore]
        [XmlAttribute(AttributeName = "typeId")]
        public Guid TypeId { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        public DashboardConfigValuePair Parameter(string name, string defaultValue)
        {
            var pair = Values.SingleOrDefault(x => x.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (pair == null)
            {
                pair = new DashboardConfigValuePair {Key = name, Value = defaultValue};
                var list = new List<DashboardConfigValuePair>(Values) {pair};
                Values = list.ToArray();
            }
            return pair;
        }
    }
}