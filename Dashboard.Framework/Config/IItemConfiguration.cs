namespace NoeticTools.Dashboard.Framework.Config
{
    public interface IItemConfiguration
    {
        /// <summary>
        ///     Configuration values.
        /// </summary>
        DashboardConfigValuePair[] Values { get; set; }

        DashboardConfigValuePair GetParameter(string name, string defaultValue);
    }
}