using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop;
using TeamCitySharp.ActionTypes;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TestDataSource
{
    public sealed class TestTeamCityClientFacade : ITcSharpTeamCityClient
    {
        public void Connect(string userName, string password)
        {
        }

        public void ConnectAsGuest()
        {
        }

        public bool Authenticate()
        {
            return true;
        }   

        public IBuilds Builds => new TestBuilds();
        public IBuildConfigs BuildConfigs => new NullInteropBuildConfigs();
        public IProjects Projects => new NullInteropProjects();
        public IServerInformation ServerInformation => new TestServerInformation();
        public IUsers Users => new NullInteripUsers();
        public IAgents Agents => new NullInteropAgents();
        public IVcsRoots VcsRoots => null;
        public IChanges Changes => null;
        public IBuildArtifacts Artifacts => null;
    }
}