using NoeticTools.Dashboard.Framework.Registries;


namespace NoeticTools.Dashboard.Framework.Tiles.Date
{
    public class Services : IServices
    {
        public Services(ITileControllerFactory tileControllerRepository, ITileProviderRegistry tileProviderRegistry, KeyboardHandler keyboardHandler)
        {
            TileProviderRegistry = tileProviderRegistry;
            TileControllerRepository = tileControllerRepository;
            KeyboardHandler = keyboardHandler;
        }

        public ITileProviderRegistry TileProviderRegistry { get; }
        public ITileControllerFactory TileControllerRepository { get; }
        public KeyboardHandler KeyboardHandler { get; }
    }
}