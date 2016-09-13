using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class AutoCompleteColourPropertyViewModel : PropertyViewModel
    {
        public AutoCompleteColourPropertyViewModel(string name, INamedValueReader tileConfigurationConverter, IServices services)
            : base(name, "AutoCompleteText", tileConfigurationConverter, () => new ColourSuggestionsProvider(services).Get().Cast<object>().ToArray())
        {
        }
    }
}