using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Input;
using NoeticTools.TeamStatusBoard.Framework.Registries;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.Framework.Styles;


namespace NoeticTools.TeamStatusBoard.Framework.Services
{
    public class ApplicationServices : IServices
    {
        private readonly IList<IService> _services = new List<IService>();

        public ApplicationServices(ITileProviderRegistry tileProviders, KeyboardHandler keyboardHandler, IPropertyEditControlRegistry propertyEditControlProviderRegistry, ITimerService timerService,
            IDataService dataService, IClock clock, IDashboardController dashboardController, IDashboardConfigurations configuration, IRunOptions runOptions)
        {
            TileProviders = tileProviders;
            KeyboardHandler = keyboardHandler;
            PropertyEditControlProviders = propertyEditControlProviderRegistry;
            Timer = timerService;
            DataService = dataService;
            Clock = clock;
            DashboardController = dashboardController;
            Configuration = configuration;
            RunOptions = runOptions;
            Style = new StatusBoardStyle();

            Register(timerService);
            Register(dataService);
        }

        public ITimerService Timer { get; }
        public IDataService DataService { get; }
        public ITileProviderRegistry TileProviders { get; }
        public IPropertyEditControlRegistry PropertyEditControlProviders { get; }
        public KeyboardHandler KeyboardHandler { get; }
        public IClock Clock { get; }
        public IDashboardController DashboardController { get; }
        public IDashboardConfigurations Configuration { get; }
        public IRunOptions RunOptions { get; }

        public IEnumerable<T> GetServicesOfType<T>()
            where T : class, IService
        {
            return _services.Where(x => x as T != null).Cast<T>().ToArray();
        }

        public T GetService<T>(string serviceName)
            where T : IService
        {
            return (T) _services.Single(x => serviceName.Equals(x.Name, StringComparison.InvariantCulture));
        }

        public IStatusBoardStyle Style { get; }

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