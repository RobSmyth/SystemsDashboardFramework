using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public interface ITileControllerRegistry
    {
        IViewController GetNew(TileConfiguration tileConfiguration);
        void Clear();
        IViewController[] GetAll();
        void Register(IViewControllerProvider provider);
    }
}