using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity
{
    public class TeamCityConfigurationSuggestionProvider : ISuggestionProvider<string>
    {
        private readonly IServices _services;

        public TeamCityConfigurationSuggestionProvider(IServices services)
        {
            _services = services;
        }

        public IEnumerable<string> Get()
        {
            var suggestions = new List<string> { "=<property>" };

            var dataSources = _services.DataService.GetAllDataSources();
            foreach (var dataSource in dataSources)
            {
                suggestions.AddRange(dataSource.Find(x => x.Tags.Contains("TeamCity.BuildConfiguration") && x.Tags.Contains("Ref")).Select(y => $"={dataSource.TypeName}.{y.Name}"));
            }

            return suggestions.OrderBy(x => x).ToArray();
        }
    }
}