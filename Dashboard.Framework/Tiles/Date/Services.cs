using NoeticTools.Dashboard.Framework.Registries;


namespace NoeticTools.Dashboard.Framework.Tiles.Date
{
    public class Services : IServices
    {
        public Services(ITileControllerRegistry repository, KeyboardHandler keyboardHandler)
        {
            Repository = repository;
            KeyboardHandler = keyboardHandler;
        }

        public ITileControllerRegistry Repository { get; }
        public KeyboardHandler KeyboardHandler { get; }
    }
}