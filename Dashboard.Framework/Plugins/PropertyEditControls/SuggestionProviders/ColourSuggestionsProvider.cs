using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders
{
    public class ColourSuggestionsProvider : ISuggestionProvider<string>
    {
        private readonly IServices _services;

        public ColourSuggestionsProvider(IServices services)
        {
            _services = services;
        }

        public IEnumerable<string> Get()
        {
            var colorProperties = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var colors = colorProperties.Select(prop => prop.Name);
            var suggestions = colors.OrderBy(x => x).ToList();

            var dataSources = _services.DataService.GetAllDataSources();
            foreach (var dataSource in dataSources)
            {
                suggestions.AddRange(dataSource.GetAllNames().Select(x => $"={dataSource.TypeName}.{x}").OrderBy(y => y));
            }

            return suggestions.ToArray();
        }
    }
}