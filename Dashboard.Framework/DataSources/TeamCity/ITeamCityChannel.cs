﻿using TeamCitySharp.DomainEntities;

namespace Dashboard.TeamCity
{
    internal interface ITeamCityChannel
    {
        void Connect();
        void Disconnect();
        Build GetLastBuild(string projectName, string buildConfigurationName);
        Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName);
        Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName, string branchName);
        Build GetRunningBuild(string projectName, string buildConfigurationName, string branchName);
        Build GetRunningBuild(string projectName, string buildConfigurationName);
    }
}