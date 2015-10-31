namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public interface IConfigurationParameter
    {
        string Name { get; set; }
        object DefaultValue { get; set; }
    }
}