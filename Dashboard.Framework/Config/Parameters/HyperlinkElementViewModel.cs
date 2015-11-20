using System.Windows.Input;


namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class HyperlinkElementViewModel : IElementViewModel
    {
        public HyperlinkElementViewModel(string text, ICommand command)
        {
            Parameters = new object[] {text, command};
            ElementType = ElementType.Hyperlink;
            Value = text;
            Name = string.Empty;
        }

        public string Name { get; }
        public ElementType ElementType { get; }
        public object[] Parameters { get; }
        public object Value { get; set; }
    }
}