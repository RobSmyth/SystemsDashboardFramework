using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders
{
    public class TextSuggestionsProvider : ISuggestionProvider<string>
    {
        private readonly IServices _services;

        public TextSuggestionsProvider(IServices services)
        {
            _services = services;
        }

        public IEnumerable<string> UpdateSuggestions()
        {
            var suggestions = new List<string> {"<literal>"};

            var dataSources = _services.DataService.GetAllDataSources();
            foreach (var dataSource in dataSources)
            {
                suggestions.AddRange(dataSource.GetAllNames().Select(x => $"={dataSource.TypeName}.{x}"));
            }

            return suggestions.OrderBy(x => x).ToArray();
        }
    }
}