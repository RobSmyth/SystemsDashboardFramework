namespace NoeticTools.Dashboard.Framework.Registries
{
    public interface IPropertyEditControlRegistry
    {
        void Register(IPropertyViewProvider provider);
        IPropertyViewProvider Get(string elementType);
    }
}