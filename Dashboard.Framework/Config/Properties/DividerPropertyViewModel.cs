namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class DividerPropertyViewModel : IPropertyViewModel
    {
        public const string Id = "Divider";

        public DividerPropertyViewModel()
        {
            Parameters = new object[0];
            EditorType = PropertyType.Divider;
            Value = null;
            Name = string.Empty;
        }

        public string Name { get; }
        public PropertyType EditorType { get; }
        public object[] Parameters { get; }
        public object Value { get; set; }
    }
}