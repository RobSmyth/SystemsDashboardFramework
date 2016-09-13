using System;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class AutoCompleteTextPropertyViewModel : PropertyViewModel
    {
        public AutoCompleteTextPropertyViewModel(string name, INamedValueReader tileConfigurationConverter, ISuggestionProvider<string> suggestionProvider) 
            : base(name, "AutoCompleteText", tileConfigurationConverter, () => suggestionProvider.UpdateSuggestions().Cast<object>().ToArray())
        {
        }
    }
}