using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Registries;
using NoeticTools.SystemsDashboard.Framework.Time;
using NoeticTools.SystemsDashboard.Framework.Registries;


namespace NoeticTools.SystemsDashboard.Framework
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