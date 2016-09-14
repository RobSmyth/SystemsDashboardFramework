using System;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class TextPropertyAutoCompleteViewModel : PropertyViewModel
    {
        public TextPropertyAutoCompleteViewModel(string name, INamedValueRepository tileConfigurationConverter, IServices services) 
            : base(name, "AutoCompleteText", tileConfigurationConverter, () => new TextSuggestionsProvider(services).Get().Cast<object>().ToArray())
        {
        }
    }
}