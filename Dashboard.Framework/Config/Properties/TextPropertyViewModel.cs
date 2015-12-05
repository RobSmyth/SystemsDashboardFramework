namespace NoeticTools.Dashboard.Framework.Config.Properties
{
    public class TextPropertyViewModel : PropertyViewModel
    {
        public TextPropertyViewModel(string name, TileConfigurationConverter tileConfigurationConverter, params string[] parameters) : base(name, "Text", tileConfigurationConverter, parameters)
        {
        }
    }
}