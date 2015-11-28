namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class PasswordElementViewModel : ElementViewModel
    {
        public PasswordElementViewModel(string name, TileConfigurationConverter tileConfigurationConverter, params string[] parameters) 
            : base(name, ElementType.Password, tileConfigurationConverter, parameters)
        {
        }
    }
}