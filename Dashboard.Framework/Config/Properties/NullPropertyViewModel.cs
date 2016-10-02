namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public sealed class NullPropertyViewModel : IPropertyViewModel
    {
        public string Name { get; }
        public PropertyType EditorType { get; }
        public object[] Parameters { get; }
        public object Value { get; set; }
    }
}