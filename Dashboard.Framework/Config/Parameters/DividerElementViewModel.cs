namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class DividerElementViewModel : IElementViewModel
    {
        public DividerElementViewModel()
        {
            Parameters = new object[0];
            ElementType = ElementType.Divider;
            Value = null;
            Name = string.Empty;
        }

        public string Name { get; }
        public ElementType ElementType { get; }
        public object[] Parameters { get; }
        public object Value { get; set; }
    }
}