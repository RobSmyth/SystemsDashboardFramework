using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders
{
    public class TextSuggestionsProvider : ISuggestionProvider<object>
    {
        private readonly IServices _services;

        public TextSuggestionsProvider(IServices services)
        {
            _services = services;
        }

        public IEnumerable<object> Get()
        {
            var suggestions = new List<ITextProperty> { new LiteralTextProperty("<literal>")};

            var dataSources = _services.DataService.GetAllDataSources();
            foreach (var dataSource in dataSources)
            {
                suggestions.AddRange(dataSource.GetAllNames().OrderBy(y => y).Select(x => new DataSourceProperty($"={dataSource.TypeName}.{x}")));
            }

            return suggestions.ToArray();
        }
    }
}