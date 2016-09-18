using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity
{
    public sealed class TeamCityConfigurationPropertyViewModel : PropertyViewModel
    {
        public TeamCityConfigurationPropertyViewModel(string name, INamedValueRepository tileConfiguration, IDataSource dataSource)
            : base(name, PropertyType.AutoCompleteText, tileConfiguration, () => dataSource.GetAllNames().Cast<object>().ToArray())
        {
        }
    }
}