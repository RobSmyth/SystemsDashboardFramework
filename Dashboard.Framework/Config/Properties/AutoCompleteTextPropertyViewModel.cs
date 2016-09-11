using System;
using System.Linq;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class AutoCompleteTextPropertyViewModel : PropertyViewModel
    {
        private readonly ISuggestionProvider<string> _suggestionProvider;

        public AutoCompleteTextPropertyViewModel(string name, TileConfigurationConverter tileConfigurationConverter, ISuggestionProvider<string> suggestionProvider) 
            : base(name, "AutoCompleteText", tileConfigurationConverter)
        {
            _suggestionProvider = suggestionProvider;
            SetParametersProvider(ParametersProvider);
        }

        private object[] ParametersProvider()
        {
            return _suggestionProvider.UpdateSuggestions(this).Cast<object>().ToArray();
        }
    }
}