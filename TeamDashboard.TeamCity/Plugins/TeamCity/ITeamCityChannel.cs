using System.Threading.Tasks;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public interface ITeamCityChannel
    {
        string[] ProjectNames { get; }
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
        Build GetLastBuild(string projectName, string buildConfigurationName);
        Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName);
        Build[] GetRunningBuilds(string projectName, string buildConfigurationName, string branchName);
        Build[] GetRunningBuilds(string projectName, string buildConfigurationName);
        Task<string[]> GetConfigurationNames(string projectName);
        Task<IBuildAgent[]> GetAgents();
        Task<IBuildAgent> GetAgent(string name);
    }
}