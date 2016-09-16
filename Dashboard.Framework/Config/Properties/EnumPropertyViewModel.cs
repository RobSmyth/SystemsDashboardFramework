using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class EnumPropertyViewModel : PropertyViewModel
    {
        public EnumPropertyViewModel(string name, INamedValueRepository tileConfiguration, params object[] values) 
            : base(name, PropertyType.Enum, tileConfiguration, () => values)
        {
        }
    }
}