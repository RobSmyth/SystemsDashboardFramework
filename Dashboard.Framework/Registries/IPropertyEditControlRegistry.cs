namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public interface IPropertyEditControlRegistry
    {
        void Register(IPropertyViewProvider provider);
        IPropertyViewProvider Get(string elementType);
    }
}