using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Configurations
{
    public sealed class BuildConfigurationRepository : IBuildConfigurationRepository
    {
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly IProject _project;
        private readonly IDictionary<string, IBuildConfiguration> _configurations = new Dictionary<string, IBuildConfiguration>();
        private readonly object _syncRoot = new object();

        public BuildConfigurationRepository(ITcSharpTeamCityClient teamCityClient, IProject project, IConnectedStateTicker connectedTicker)
        {
            _teamCityClient = teamCityClient;
            _project = project;
            connectedTicker.AddListener(Update);
        }

        private void Update()
        {
            try
            {
                lock (_syncRoot)
                {
                    var tcsConfigurations = _teamCityClient.BuildConfigs.ByProjectId(_project.Id).ToArray();
                    var found = new List<IBuildConfiguration>();
                    foreach (var tcsConfiguration in tcsConfigurations)
                    {
                        var normalisedName = tcsConfiguration.Name.ToLower();
                        if (!_configurations.ContainsKey(normalisedName))
                        {
                            _configurations.Add(normalisedName, new BuildConfiguration(tcsConfiguration, _project, _teamCityClient));
                        }
                        else
                        {
                            _configurations[normalisedName].Update(tcsConfiguration);
                        }
                        found.Add(_configurations[normalisedName]);
                    }

                    foreach (var orphan in _configurations.Values.Except(found).ToArray())
                    {
                        orphan.Update(new NullInteropBuildConfig(orphan.Name));
                    }
                }
            }
            catch (Exception)
            {
                // todo - log exception
            }
        }

        public IBuildConfiguration[] GetAll()
        {
            return _configurations.Values.ToArray();
        }

        public IBuildConfiguration Get(string name)
        {
            var normalisedName = name.ToLower();
            if (!_configurations.ContainsKey(normalisedName))
            {
                _configurations.Add(normalisedName, new BuildConfiguration(new BuildConfig() {Name = name, Description = "n/a", Id="n/a"}, _project, _teamCityClient));
            }
            return _configurations[normalisedName];
        }
    }
}
