using System;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class AutoCompleteTextPropertyViewModel : PropertyViewModel
    {
        public AutoCompleteTextPropertyViewModel(string name, INamedValueReader tileConfigurationConverter, IServices services) 
            : base(name, "AutoCompleteText", tileConfigurationConverter, () => new TextSuggestionsProvider(services).Get().Cast<object>().ToArray())
        {
        }
    }
}