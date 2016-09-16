using System;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class PasswordPropertyViewModel : PropertyViewModel
    {
        public PasswordPropertyViewModel(string name, INamedValueRepository tileConfiguration, Func<object[]> parametersFunc = null)
            : base(name, PropertyType.Password, tileConfiguration, parametersFunc)
        {
        }
    }
}