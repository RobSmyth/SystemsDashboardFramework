using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;


namespace NoeticTools.TeamStatusBoard.DataSources.DashboardData
{
    public class DashboardDataConfiguration : IItemConfiguration
    {
        private readonly DashboardServiceConfiguration _inner;

        public DashboardDataConfiguration(DashboardServiceConfiguration inner)
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