using TeamCitySharp.DomainEntities;


namespace NoeticTools.Dashboard.Framework.DataSources.TeamCity
{
    internal interface ITeamCityChannel
    {
        void Connect();
        void Disconnect();
        Build GetLastBuild(string projectName, string buildConfigurationName);
        Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName);
        Build GetRunningBuild(string projectName, string buildConfigurationName, string branchName);
        Build GetRunningBuild(string projectName, string buildConfigurationName);
        string[] ProjectNames { get; }
        string[] GetConfigurationNames(string projectName);
    }
}