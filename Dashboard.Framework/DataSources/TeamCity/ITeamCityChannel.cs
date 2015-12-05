using TeamCitySharp.DomainEntities;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    internal interface ITeamCityChannel
    {
        string[] ProjectNames { get; }
        void Connect();
        void Disconnect();
        Build GetLastBuild(string projectName, string buildConfigurationName);
        Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName);
        Build GetRunningBuild(string projectName, string buildConfigurationName, string branchName);
        Build GetRunningBuild(string projectName, string buildConfigurationName);
        string[] GetConfigurationNames(string projectName);
    }
}