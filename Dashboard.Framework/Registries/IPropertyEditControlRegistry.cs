using NoeticTools.TeamStatusBoard.Framework.Config.Properties;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public interface IPropertyEditControlRegistry
    {
        void Register(IPropertyViewProvider provider);
        IPropertyViewProvider Get(PropertyType elementType);
    }
}