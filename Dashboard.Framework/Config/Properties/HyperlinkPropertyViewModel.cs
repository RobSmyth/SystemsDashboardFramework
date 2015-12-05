using System.Windows.Input;


namespace NoeticTools.SystemsDashboard.Framework.Config.Properties
{
    public class HyperlinkPropertyViewModel : IPropertyViewModel
    {
        public HyperlinkPropertyViewModel(string text, ICommand command)
        {
            Parameters = new object[] {text, command};
            ViewerName = "Hyperlink";
            Value = text;
            Name = string.Empty;
        }

        public string Name { get; }
        public string ViewerName { get; }
        public object[] Parameters { get; set; }
        public object Value { get; set; }
    }
}