using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class CompountColourPropertyViewModel : PropertyViewModel
    {
        public CompountColourPropertyViewModel(string name, INamedValueRepository tileConfiguration, IServices services)
            : base(name, PropertyType.CompoundAutoCompleteText, tileConfiguration, () => new ColourSuggestionsProvider(services).Get().Cast<object>().ToArray())
        {
        }
    }
}