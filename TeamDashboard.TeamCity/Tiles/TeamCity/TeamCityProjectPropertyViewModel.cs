using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity
{
    public sealed class TeamCityProjectPropertyViewModel : PropertyViewModel
    {
        public TeamCityProjectPropertyViewModel(string name, TileConfigurationConverter tileConfigurationConverter, ITeamCityChannel channel)
            : base(name, "TextFromCombobox", tileConfigurationConverter, () => channel.ProjectNames.Cast<object>().ToArray())
        {
        }
    }
}