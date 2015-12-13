using System.Threading.Tasks;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    internal interface ITeamCityChannel
    {
        string[] ProjectNames { get; }
        bool IsConnected { get; }
        void Connect();
        Task<Build> GetLastBuild(string projectName, string buildConfigurationName);
        Task<Build> GetLastSuccessfulBuild(string projectName, string buildConfigurationName);
        Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName, string branchName);
        Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName);
        Task<string[]> GetConfigurationNames(string projectName);
        Task<IBuildAgent[]> GetAgents();
        Task<IBuildAgent> GetAgent(string name);
    }
}