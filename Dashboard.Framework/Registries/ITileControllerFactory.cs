using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public interface ITileControllerFactory
    {
        IViewController Create(TileConfiguration tileConfiguration);
    }
}