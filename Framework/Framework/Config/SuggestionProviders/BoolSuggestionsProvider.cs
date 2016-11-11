using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders
{
    public class BoolSuggestionsProvider : ISuggestionProvider<object>
    {
        private readonly IServices _services;

        public BoolSuggestionsProvider(IServices services)
        {
            _services = services;
        }

        public IEnumerable<object> Get()
        {
            var suggestions = new List<ITextProperty>
            {
                new LiteralTextProperty(true.ToString()),
                new LiteralTextProperty(false.ToString()),
            };

            var dataSources = _services.DataService.GetAllDataSources();
            foreach (var dataSource in dataSources)
            {
                suggestions.AddRange(dataSource.GetAllNames().Select(x => new LiteralTextProperty($"={dataSource.TypeName}.{x}")).OrderBy(y => y));
            }

            return suggestions.ToArray();
        }
    }
}