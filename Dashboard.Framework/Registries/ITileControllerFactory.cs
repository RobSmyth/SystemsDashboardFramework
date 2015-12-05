using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Registries
{
    public interface ITileControllerFactory
    {
        IViewController Create(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController);
    }
}