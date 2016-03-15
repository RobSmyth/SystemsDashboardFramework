using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.StaticProperties
{
    public class VizBoardPropertiesServicesConfiguration : IItemConfiguration
    {
        private readonly DashboardServiceConfiguration _inner;

        public VizBoardPropertiesServicesConfiguration(DashboardServiceConfiguration inner)
        {
            _inner = inner;
        }

        public DashboardConfigValuePair[] Values
        {
            get { return _inner.Values; }
            set { _inner.Values = value; }
        }

        public DashboardConfigValuePair GetParameter(string name, string defaultValue)
        {
            return _inner.GetParameter(name, defaultValue);
        }
    }
}