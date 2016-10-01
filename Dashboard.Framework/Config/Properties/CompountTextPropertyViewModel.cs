using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class CompountTextPropertyViewModel : PropertyViewModel
    {
        public CompountTextPropertyViewModel(string name, INamedValueRepository tileConfiguration, IServices services)
            : base(name, PropertyType.CompoundAutoCompleteText, tileConfiguration, () => new TextSuggestionsProvider(services).Get().Cast<object>().ToArray())
        {
        }
    }
}