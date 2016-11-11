using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity
{

    public sealed class TeamCityConfigurationPropertyViewModel : PropertyViewModel
    {
        public TeamCityConfigurationPropertyViewModel(string name, INamedValueRepository tileConfiguration, IServices services)
            : base(name, PropertyType.AutoCompleteText, tileConfiguration, new TeamCityConfigurationSuggestionProvider(services))
        {
        }
    }
}