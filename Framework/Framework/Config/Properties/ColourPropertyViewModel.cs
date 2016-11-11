using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class ColourPropertyViewModel : PropertyViewModel
    {
        public ColourPropertyViewModel(string name, INamedValueRepository tileConfiguration, IServices services)
            : base(name, PropertyType.AutoCompleteText, tileConfiguration, new ColourSuggestionsProvider(services))
        {
        }
    }
}