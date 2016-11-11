using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class CompoundColourPropertyViewModel : PropertyViewModel
    {
        public CompoundColourPropertyViewModel(string name, INamedValueRepository tileConfiguration, IServices services)
            : base(name, PropertyType.CompoundAutoCompleteText, tileConfiguration, new ColourSuggestionsProvider(services))
        {
        }
    }
}