using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.ActionTypes;
using TeamCitySharp.Locators;


namespace Dashboard.TeamCity
{
    interface ITeamCityChannel
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
