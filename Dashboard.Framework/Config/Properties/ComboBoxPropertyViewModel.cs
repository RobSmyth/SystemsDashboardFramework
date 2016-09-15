using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class EnumPropertyViewModel : PropertyViewModel
    {
        public EnumPropertyViewModel(string name, INamedValueRepository tileConfigurationConverter, params object[] values) 
            : base(name, "TextFromCombobox", tileConfigurationConverter, () => values)
        {
        }
    }
}