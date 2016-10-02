using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class HyperlinkPropertyViewModel : IPropertyViewModel
    {
        public const string Id = "Hyperlink";
        public HyperlinkPropertyViewModel(string text, ICommand command)
        {
            Parameters = new object[] {text, command};
            EditorType = PropertyType.Hyperlink;
            Value = new DataValue("Hyperlink", text, PropertiesFlags.None, () => {});
            Name = string.Empty;
        }

        public string Name { get; }
        public PropertyType EditorType { get; }
        public object[] Parameters { get; }
        public IDataValue Value { get; set; }
    }
}