using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;


namespace NoeticTools.TeamStatusBoard.Framework.Config
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