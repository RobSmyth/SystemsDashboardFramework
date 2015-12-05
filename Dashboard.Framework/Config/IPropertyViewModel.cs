namespace NoeticTools.Dashboard.Framework.Config
{
    public interface IPropertyViewModel
    {
        string Name { get; }
        /// <summary>
        /// Name of view provider that will be used to create the view element (e.g. TextBox).
        /// </summary>
        string ViewerName { get; }
        object[] Parameters { get; set; }
        object Value { get; set; }
    }
}