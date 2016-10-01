using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class BoolPropertyViewModel : PropertyViewModel
    {
        public BoolPropertyViewModel(string name, INamedValueRepository tileConfiguration, IServices services)
            : base(name, PropertyType.AutoCompleteText, tileConfiguration, () => new BoolSuggestionsProvider(services).Get().Cast<object>().ToArray())
        {
        }
    }
}