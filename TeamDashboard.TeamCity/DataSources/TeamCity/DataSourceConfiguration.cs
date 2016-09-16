using NoeticTools.TeamStatusBoard.Framework.Config;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity
{
    public sealed class DataSourceConfiguration : ITeamCityDataSourceConfiguration
    {
        private readonly DashboardServiceConfiguration _inner;

        public DataSourceConfiguration(DashboardServiceConfiguration inner)
        {
            _inner = inner;
        }

        public string Url
        {
            get { return _inner.Parameter("Url", "<your_teamcity_server>:8080").Value; }
            set { _inner.Parameter("Url", "<your_teamcity_server>:8080").Value = value; }
        }

        public string UserName
        {
            get { return _inner.Parameter("UserName", "<your_username>").Value; }
            set { _inner.Parameter("UserName", "<your_username>").Value = value; }
        }

        public string Password
        {
            get { return _inner.Parameter("Password", "MyPassword").Value; }
            set { _inner.Parameter("Password", "MyPassword").Value = value; }
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