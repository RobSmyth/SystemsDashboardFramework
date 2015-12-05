using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Properties;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.TeamCity
{
    public sealed class TeamCityProjectPropertyViewModel : PropertyViewModel
    {
        public TeamCityProjectPropertyViewModel(string name, TileConfigurationConverter tileConfigurationConverter, TeamCityService service)
            : base(name, "TextFromCombobox", tileConfigurationConverter, service.ProjectNames)
        {
        }
    }
}