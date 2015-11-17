using NoeticTools.Dashboard.Framework.Tiles.Date;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface IPlugin
    {
        void Register(IServices services);
    }
}