using NoeticTools.TeamStatusBoard.Framework.Config.Properties;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public sealed class NullPropertyViewModel : IPropertyViewModel
    {
        public string Name { get; }
        public PropertyType EditorType { get; }
        public object[] Parameters { get; }
        public object Value { get; set; }
    }
}