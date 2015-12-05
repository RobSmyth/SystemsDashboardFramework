namespace NoeticTools.SystemsDashboard.Framework.Config.Properties
{
    public class PasswordPropertyViewModel : PropertyViewModel
    {
        public PasswordPropertyViewModel(string name, TileConfigurationConverter tileConfigurationConverter, params string[] parameters)
            : base(name, "Password", tileConfigurationConverter, parameters)
        {
        }
    }
}