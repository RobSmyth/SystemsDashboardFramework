using System.Linq;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.DataSources.TeamCity;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.TeamCity
{
    public sealed class TeamCityProjectPropertyViewModel : PropertyViewModel
    {
        public TeamCityProjectPropertyViewModel(string name, TileConfigurationConverter tileConfigurationConverter, TeamCityService service)
            : base(name, "TextFromCombobox", tileConfigurationConverter, () => service.ProjectNames.Cast<object>().ToArray())
        {
        }
    }
}