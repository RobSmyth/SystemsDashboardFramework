using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;


namespace NoeticTools.SystemsDashboard.Framework.Config
{
    public abstract class ItemConfigurationBase : IItemConfiguration
    {
        /// <summary>
        ///     Configuration values.
        /// </summary>
        [XmlArray(ElementName = "values")]
        public DashboardConfigValuePair[] Values { get; set; }

        public DashboardConfigValuePair GetParameter(string name, string defaultValue)
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