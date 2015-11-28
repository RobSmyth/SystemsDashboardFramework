namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class TextElementViewModel : ElementViewModel
    {
        public TextElementViewModel(string name, TileConfigurationConverter tileConfigurationConverter, params string[] parameters) : base(name, ElementType.Text, tileConfigurationConverter, parameters)
        {
        }
    }
}