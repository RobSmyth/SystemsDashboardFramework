using System;


namespace NoeticTools.SystemsDashboard.Framework.Config.Properties
{
    public class PasswordPropertyViewModel : PropertyViewModel
    {
        public PasswordPropertyViewModel(string name, TileConfigurationConverter tileConfigurationConverter, Func<object[]> parametersFunc = null)
            : base(name, "Password", tileConfigurationConverter, parametersFunc)
        {
        }
    }
}