using NoeticTools.Dashboard.Framework.Input;
using NoeticTools.Dashboard.Framework.Registries;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework
{
    public class Services : IServices
    {
        public Services(ITileProviderRegistry tileProviders, KeyboardHandler keyboardHandler, IPropertyEditControlRegistry propertyEditControlProviderRegistry, TimerService timerService)
        {
            TileProviders = tileProviders;
            KeyboardHandler = keyboardHandler;
            PropertyEditControlProviders = propertyEditControlProviderRegistry;
            Timer = timerService;
        }

        public ITimerService Timer { get; }
        public ITileProviderRegistry TileProviders { get; }
        public IPropertyEditControlRegistry PropertyEditControlProviders { get; }
        public KeyboardHandler KeyboardHandler { get; }
    }
}