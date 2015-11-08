namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public interface IConfigurationParameter
    {
        string Name { get; set; }
        ElementType ValueType { get; set; }
    }
}