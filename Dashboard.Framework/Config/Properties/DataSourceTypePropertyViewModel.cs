using System;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class DataSourceTypePropertyViewModel : PropertyViewModel
    {
        public DataSourceTypePropertyViewModel(string name, INamedValueRepository tileConfiguration, IServices services) 
            : base(name, PropertyType.Enum, tileConfiguration, () => services.DataService.GetAllDataSources().Select(x => x.TypeName).Cast<object>().ToArray())
        {
        }
    }
}