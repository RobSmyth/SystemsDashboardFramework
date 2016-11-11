using System;
using System.Collections.Generic;
using TeamCitySharp.ActionTypes;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop
{
    public sealed class NullInteropBuilds : IBuilds
    {
        public List<Build> SuccessfulBuildsByBuildConfigId(string buildConfigId)
        {
            return new List<Build>();
        }

        public Build LastSuccessfulBuildByBuildConfigId(string buildConfigId)
        {
            return null;
        }

        public List<Build> FailedBuildsByBuildConfigId(string buildConfigId)
        {
            return new List<Build>();
        }

        public Build LastFailedBuildByBuildConfigId(string buildConfigId)
        {
            return null;
        }

        public Build LastBuildByBuildConfigId(string buildConfigId)
        {
            return null;
        }

        public List<Build> ErrorBuildsByBuildConfigId(string buildConfigId)
        {
            return new List<Build>();
        }

        public Build LastErrorBuildByBuildConfigId(string buildConfigId)
        {
            return null;
        }

        public List<Build> ByBuildConfigId(string buildConfigId)
        {
            return new List<Build>();
        }

        public List<Build> ByConfigIdAndTag(string buildConfigId, string tag)
        {
            return new List<Build>();
        }

        public List<Build> ByUserName(string userName)
        {
            return new List<Build>();
        }

        public List<Build> ByBuildLocator(BuildLocator locator)
        {
            return new List<Build>();
        }

        public List<Build> NonSuccessfulBuildsForUser(string userName)
        {
            return new List<Build>();
        }

        public List<Build> ByBranch(string branchName)
        {
            return new List<Build>();
        }

        public Build LastBuildByAgent(string agentName)
        {
            return null;
        }

        public void Add2QueueBuildByBuildConfigId(string buildConfigId)
        {
        }

        public List<Build> AllBuildsOfStatusSinceDate(DateTime date, BuildStatus buildStatus)
        {
            return new List<Build>();
        }

        public List<Build> AllSinceDate(DateTime date)
        {
            return new List<Build>();
        }
    }
}