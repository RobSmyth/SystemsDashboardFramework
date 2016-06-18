using System;
using System.Collections.Generic;
using System.Xml;
using TeamCitySharp.ActionTypes;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop
{
    public sealed class NullInteropBuildConfigs : IBuildConfigs
    {
        public List<BuildConfig> All()
        {
            return new List<BuildConfig>();
        }

        public BuildConfig ByConfigurationName(string buildConfigName)
        {
            throw new System.NotImplementedException();
        }

        public BuildConfig ByConfigurationId(string buildConfigId)
        {
            throw new System.NotImplementedException();
        }

        public BuildConfig ByProjectNameAndConfigurationName(string projectName, string buildConfigName)
        {
            throw new System.NotImplementedException();
        }

        public BuildConfig ByProjectNameAndConfigurationId(string projectName, string buildConfigId)
        {
            throw new System.NotImplementedException();
        }

        public BuildConfig ByProjectIdAndConfigurationName(string projectId, string buildConfigName)
        {
            throw new System.NotImplementedException();
        }

        public BuildConfig ByProjectIdAndConfigurationId(string projectId, string buildConfigId)
        {
            throw new System.NotImplementedException();
        }

        public List<BuildConfig> ByProjectId(string projectId)
        {
            return new List<BuildConfig>();
        }

        public List<BuildConfig> ByProjectName(string projectName)
        {
            return new List<BuildConfig>();
        }

        public BuildConfig CreateConfiguration(string projectName, string configurationName)
        {
            throw new System.NotImplementedException();
        }

        public void SetConfigurationSetting(BuildTypeLocator locator, string settingName, string settingValue)
        {
        }

        public bool GetConfigurationPauseStatus(BuildTypeLocator locator)
        {
            return false;
        }

        public void SetConfigurationPauseStatus(BuildTypeLocator locator, bool isPaused)
        {
        }

        public void PostRawArtifactDependency(BuildTypeLocator locator, string rawXml)
        {
        }

        public void PostRawBuildStep(BuildTypeLocator locator, string rawXml)
        {
        }

        public void PostRawBuildTrigger(BuildTypeLocator locator, string rawXml)
        {
        }

        public void SetConfigurationParameter(BuildTypeLocator locator, string key, string value)
        {
        }

        public void PostRawAgentRequirement(BuildTypeLocator locator, string rawXml)
        {
        }

        public void DeleteBuildStep(BuildTypeLocator locator, string buildStepId)
        {
        }

        public void DeleteArtifactDependency(BuildTypeLocator locator, string artifactDependencyId)
        {
        }

        public void DeleteAgentRequirement(BuildTypeLocator locator, string agentRequirementId)
        {
        }

        public void DeleteParameter(BuildTypeLocator locator, string parameterName)
        {
        }

        public void DeleteBuildTrigger(BuildTypeLocator locator, string buildTriggerId)
        {
        }

        public void SetBuildTypeTemplate(BuildTypeLocator locatorBuildType, BuildTypeLocator locatorTemplate)
        {
        }

        public void DeleteSnapshotDependency(BuildTypeLocator locator, string snapshotDependencyId)
        {
        }

        public BuildConfig BuildType(BuildTypeLocator locator)
        {
            return null;
        }

        public void DeleteConfiguration(BuildTypeLocator locator)
        {
        }

        public void DeleteAllBuildTypeParameters(BuildTypeLocator locator)
        {
        }

        public void DownloadConfiguration(BuildTypeLocator locator, Action<string> downloadHandler)
        {
        }

        public void PutAllBuildTypeParameters(BuildTypeLocator locator, IDictionary<string, string> parameters)
        {
        }

        public void PostRawSnapshotDependency(BuildTypeLocator locator, XmlElement rawXml)
        {
        }
    }
}