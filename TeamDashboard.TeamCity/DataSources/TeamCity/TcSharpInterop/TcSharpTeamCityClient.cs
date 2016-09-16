using System;
using log4net;
using TeamCitySharp;
using TeamCitySharp.ActionTypes;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop
{
    public class TcSharpTeamCityClient : ITcSharpTeamCityClient
    {
        private readonly TeamCityClient _inner;
        private readonly ILog _logger;

        public TcSharpTeamCityClient(TeamCityClient inner)
        {
            _inner = inner;
            _logger = LogManager.GetLogger("DateSources.TeamCity.Client");
        }

        public IBuilds Builds
        {
            get { return SafeInvokeWithNullType<IBuilds, NullInteropBuilds>(() => _inner.Builds); }
        }

        public IBuildConfigs BuildConfigs
        {
            get { return SafeInvokeWithNullType<IBuildConfigs, NullInteropBuildConfigs>(() => _inner.BuildConfigs); }
        }

        public IProjects Projects
        {
            get { return SafeInvokeWithNullType<IProjects, NullInteropProjects>(() => _inner.Projects ); }
        }

        public IServerInformation ServerInformation
        {
            get { return SafeInvokeWithDefault(() => _inner.ServerInformation); }
        }

        public IUsers Users
        {
            get { return SafeInvokeWithNullType<IUsers, NullInteripUsers>(() => _inner.Users); }
        }

        public IAgents Agents
        {
            get { return new TxSharpAgents(SafeInvokeWithNullType<IAgents, NullInteropAgents>(() => _inner.Agents)); }
        }

        public IVcsRoots VcsRoots
        {
            get { return SafeInvokeWithDefault(() => _inner.VcsRoots); }
        }

        public IChanges Changes
        {
            get { return SafeInvokeWithDefault(() => _inner.Changes); }
        }

        public IBuildArtifacts Artifacts
        {
            get { return SafeInvokeWithDefault(() => _inner.Artifacts); }
        }

        public void Connect(string userName, string password)
        {
            SafeInvoke(() => _inner.Connect(userName, password));
        }

        public void ConnectAsGuest()
        {
            SafeInvoke(() => _inner.ConnectAsGuest());
        }

        public bool Authenticate()
        {
            return SafeInvokeWithDefault(() => _inner.Authenticate());
        }

        private T SafeInvokeWithNullType<T,TN>(Func<T> func)
            where TN : class, T, new()
        {
            try
            {
                return func();
            }
            catch (Exception exception)
            {
                _logger.Error("Exception", exception);
                return new TN();
            }
        }

        private T SafeInvokeWithDefault<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception exception)
            {
                _logger.Error("Exception", exception);
                return default(T);
            }
        }

        private void SafeInvoke(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                _logger.Error("Exception", exception);
            }
        }
    }
}