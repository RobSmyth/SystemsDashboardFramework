using System;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class DataSourceTypePropertyViewModel : PropertyViewModel
    {
        public DataSourceTypePropertyViewModel(string name, INamedValueReader tileConfigurationConverter, IServices services) 
            : base(name, "TextFromCombobox", tileConfigurationConverter, () => services.DataService.GetAllDataSources().Select(x => x.TypeName).Cast<object>().ToArray())
        {
        }
    }
}