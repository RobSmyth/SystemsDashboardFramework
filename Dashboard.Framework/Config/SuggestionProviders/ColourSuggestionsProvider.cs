using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Framework.Config.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders
{
    public class ColourSuggestionsProvider : ISuggestionProvider<object>
    {
        private readonly IServices _services;

        public ColourSuggestionsProvider(IServices services)
        {
            _services = services;
        }

        public IEnumerable<object> Get()
        {
            var suggestions = new List<ITextProperty>();
            var colorProperties = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var colors = colorProperties.Select(prop => prop.Name);
            suggestions.AddRange(colors.Select(y => new ColourNameProperty(y)).OrderBy(x => x));

            var dataSources = _services.DataService.GetAllDataSources();
            foreach (var dataSource in dataSources)
            {
                suggestions.AddRange(dataSource.GetAllNames().Select(x => new DataSourceProperty($"={dataSource.TypeName}.{x}")).OrderBy(y => y));
            }

            return suggestions.ToArray();
        }
    }
}