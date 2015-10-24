﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Dashboard.Config
{
    [XmlType("service")]
    public class DashboardServiceConfiguration
    {
        private const string DefaultGuid = "B714E5AB-8F0D-4C4A-A6CB-44B6C3AEEE88";

        public DashboardServiceConfiguration()
        {
            TypeId = Guid.Parse(DefaultGuid);
            Name = string.Empty;
            Values = new[] {new DashboardConfigValuePair {Key = "A", Value = "1"}};
        }

        [XmlAttribute(AttributeName = "typeId")]
        public Guid TypeId { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlArray(ElementName = "values")]
        public DashboardConfigValuePair[] Values { get; set; }

        public DashboardConfigValuePair Parameter(string name, string defaultValue)
        {
            DashboardConfigValuePair pair =
                Values.SingleOrDefault(x => x.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (pair == null)
            {
                pair = new DashboardConfigValuePair {Key = name, Value = defaultValue};
                var list = new List<DashboardConfigValuePair>(Values);
                list.Add(pair);
                Values = list.ToArray();
            }
            return pair;
        }
    }
}