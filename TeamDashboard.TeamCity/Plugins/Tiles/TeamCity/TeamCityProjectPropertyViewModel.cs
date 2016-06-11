using System.Linq;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.Tiles.TeamCity
{
    public sealed class TeamCityProjectPropertyViewModel : PropertyViewModel
    {
        public TeamCityProjectPropertyViewModel(string name, TileConfigurationConverter tileConfigurationConverter, TeamCityService service)
            : base(name, "TextFromCombobox", tileConfigurationConverter, () => service.ProjectNames.Cast<object>().ToArray())
        {
        }
    }
}