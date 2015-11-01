using NoeticTools.Dashboard.Framework.Config;


namespace Dashboard.Services.TeamCity
{
    public class TeamCityServiceConfiguration
    {
        private readonly DashboardServiceConfiguration _inner;

        public TeamCityServiceConfiguration(DashboardServiceConfiguration inner)
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
    }
}