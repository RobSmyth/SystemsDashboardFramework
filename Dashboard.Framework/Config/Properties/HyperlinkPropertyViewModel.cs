using System.Windows.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class HyperlinkPropertyViewModel : IPropertyViewModel
    {
        public const string Id = "Hyperlink";
        public HyperlinkPropertyViewModel(string text, ICommand command)
        {
            Parameters = new object[] {text, command};
            EditorType = PropertyType.Hyperlink;
            Value = text;
            Name = string.Empty;
        }

        public string Name { get; }
        public PropertyType EditorType { get; }
        public object[] Parameters { get; }
        public object Value { get; set; }
    }
}