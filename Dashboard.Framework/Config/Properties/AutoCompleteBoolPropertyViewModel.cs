using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class AutoCompleteBoolPropertyViewModel : PropertyViewModel
    {
        public AutoCompleteBoolPropertyViewModel(string name, INamedValueReader tileConfigurationConverter, IServices services)
            : base(name, "AutoCompleteText", tileConfigurationConverter, () => new BoolSuggestionsProvider(services).UpdateSuggestions().Cast<object>().ToArray())
        {
        }
    }
}