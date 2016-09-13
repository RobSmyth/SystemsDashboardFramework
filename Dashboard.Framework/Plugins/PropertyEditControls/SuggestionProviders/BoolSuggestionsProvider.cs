using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders
{
    public class BoolSuggestionsProvider : ISuggestionProvider<string>
    {
        private readonly IServices _services;

        public BoolSuggestionsProvider(IServices services)
        {
            _services = services;
        }

        public IEnumerable<string> UpdateSuggestions()
        {
            var suggestions = new List<string> { true.ToString(), false.ToString() };

            var dataSources = _services.DataService.GetAllDataSources();
            foreach (var dataSource in dataSources)
            {
                suggestions.AddRange(dataSource.GetAllNames().Select(x => $"={dataSource.TypeName}.{x}").OrderBy(y => y));
            }

            return suggestions.ToArray();
        }
    }
}