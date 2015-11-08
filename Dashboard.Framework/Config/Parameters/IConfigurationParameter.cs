namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public interface IConfigurationParameter
    {
        string Name { get; }
        ElementType ElementType { get; }
    }
}