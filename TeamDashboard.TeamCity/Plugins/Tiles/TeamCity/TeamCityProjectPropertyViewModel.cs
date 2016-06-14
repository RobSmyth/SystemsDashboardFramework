using System.Linq;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.Tiles.TeamCity
{
    public sealed class TeamCityProjectPropertyViewModel : PropertyViewModel
    {
        public TeamCityProjectPropertyViewModel(string name, TileConfigurationConverter tileConfigurationConverter, ITeamCityChannel channel)
            : base(name, "TextFromCombobox", tileConfigurationConverter, () => channel.ProjectNames.Cast<object>().ToArray())
        {
        }
    }
}