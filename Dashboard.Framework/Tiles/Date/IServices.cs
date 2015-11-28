using NoeticTools.Dashboard.Framework.Registries;


namespace NoeticTools.Dashboard.Framework.Tiles.Date
{
    public interface IServices
    {
        ITileControllerFactory TileControllerRepository { get; }
        KeyboardHandler KeyboardHandler { get; }
        ITileProviderRegistry TileProviderRegistry { get; }
    }
}