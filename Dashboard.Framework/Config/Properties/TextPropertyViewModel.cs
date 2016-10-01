using System;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class TextPropertyViewModel : PropertyViewModel
    {
        public TextPropertyViewModel(string name, INamedValueRepository tileConfiguration, IServices services) 
            : base(name, PropertyType.AutoCompleteText, tileConfiguration, new TextSuggestionsProvider(services))
        {
        }
    }
}