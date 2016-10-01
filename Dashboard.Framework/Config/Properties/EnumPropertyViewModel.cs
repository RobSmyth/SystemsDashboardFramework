using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class EnumPropertyViewModel : PropertyViewModel
    {
        public EnumPropertyViewModel(string name, INamedValueRepository tileConfiguration, ISuggestionProvider<object> suggestionProvider) 
            : base(name, PropertyType.Enum, tileConfiguration, suggestionProvider)
        {
        }
    }
}