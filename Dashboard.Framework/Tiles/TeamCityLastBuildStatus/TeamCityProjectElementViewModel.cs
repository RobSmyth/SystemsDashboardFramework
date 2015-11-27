using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;


namespace NoeticTools.Dashboard.Framework.Tiles.TeamCityLastBuildStatus
{
    public class TeamCityProjectElementViewModel : ElementViewModel
    {
        public TeamCityProjectElementViewModel(string name, TileConfigurationConverter tileConfigurationConverter, TeamCityService service) 
            : base(name, ElementType.SelectedText, tileConfigurationConverter, service.ProjectNames)
        {
        }
    }
}