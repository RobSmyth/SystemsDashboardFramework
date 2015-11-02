using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface ITileRegistry
    {
        IViewController GetNew(TileConfiguration tileConfiguration);
        void Clear();
        IViewController[] GetAll();
    }
}