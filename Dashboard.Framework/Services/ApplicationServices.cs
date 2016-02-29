using System.Collections.Generic;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Registries;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;


namespace NoeticTools.SystemsDashboard.Framework.Services
{
    public class ApplicationServices : IServices
    {
        private readonly IList<IService> _services = new List<IService>();

        public ApplicationServices(ITileProviderRegistry tileProviders, KeyboardHandler keyboardHandler, 
            IPropertyEditControlRegistry propertyEditControlProviderRegistry, 
            ITimerService timerService, 
            IDataService dataService)
        {
            TileProviders = tileProviders;
            KeyboardHandler = keyboardHandler;
            PropertyEditControlProviders = propertyEditControlProviderRegistry;
            Timer = timerService;
            DataService = dataService;

            Register(timerService);
            Register(dataService);
        }

        public ITimerService Timer { get; }
        public IDataService DataService { get; }
        public ITileProviderRegistry TileProviders { get; }
        public IPropertyEditControlRegistry PropertyEditControlProviders { get; }
        public KeyboardHandler KeyboardHandler { get; }

        public void Register(IService service)
        {
            _services.Add(service);
        }

        public void Stop()
        {
            foreach (var service in _services)
            {
                service.Stop();
            }
        }

        public void Start()
        {
            foreach (var service in _services)
            {
                service.Start();
            }
        }
    }
}