using TeamCitySharp;
using TeamCitySharp.ActionTypes;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop
{
    public class TcSharpTeamCityClient : ITcSharpTeamCityClient
    {
        private readonly TeamCityClient _inner;

        public TcSharpTeamCityClient(TeamCityClient inner)
        {
            _inner = inner;
        }

        public void Connect(string userName, string password)
        {
            _inner.Connect(userName, password);
        }

        public void ConnectAsGuest()
        {
            _inner.ConnectAsGuest();
        }

        public bool Authenticate()
        {
            return _inner.Authenticate();
        }

        public IBuilds Builds
        {
            get { return _inner.Builds; }
        }

        public IBuildConfigs BuildConfigs
        {
            get { return _inner.BuildConfigs; }
        }

        public IProjects Projects
        {
            get { return _inner.Projects; }
        }

        public IServerInformation ServerInformation
        {
            get { return _inner.ServerInformation; }
        }

        public IUsers Users
        {
            get { return _inner.Users; }
        }

        public IAgents Agents
        {
            get { return _inner.Agents; }
        }

        public IVcsRoots VcsRoots
        {
            get { return _inner.VcsRoots; }
        }

        public IChanges Changes
        {
            get { return _inner.Changes; }
        }

        public IBuildArtifacts Artifacts
        {
            get { return _inner.Artifacts; }
        }

        public ITestOccurrences TestOccurrences
        {
            get { return _inner.TestOccurrences; }
        }

        public IStatistics Statistics
        {
            get { return _inner.Statistics; }
        }
    }
}
