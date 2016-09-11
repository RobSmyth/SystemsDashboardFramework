namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class DividerPropertyViewModel : IPropertyViewModel
    {
        public DividerPropertyViewModel()
        {
            Parameters = new object[0];
            ViewerName = "Divider";
            Value = null;
            Name = string.Empty;
        }

        public string Name { get; }
        public string ViewerName { get; }
        public object[] Parameters { get; set; }
        public object Value { get; set; }
        public void UpdateParameters()
        {
            throw new System.NotImplementedException();
        }
    }
}