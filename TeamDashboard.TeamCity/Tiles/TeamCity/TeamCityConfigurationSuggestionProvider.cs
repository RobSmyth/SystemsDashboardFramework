using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity
{
    public class TeamCityConfigurationSuggestionProvider : ISuggestionProvider<object>
    {
        private readonly IServices _services;

        public TeamCityConfigurationSuggestionProvider(IServices services)
        {
            _services = services;
        }

        public IEnumerable<object> Get()
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