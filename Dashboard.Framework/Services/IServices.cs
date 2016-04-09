using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Registries;
using NoeticTools.SystemsDashboard.Framework.Services;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.Framework.Registries;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Services
{
    public interface IServices
    {
        KeyboardHandler KeyboardHandler { get; }
        ITileProviderRegistry TileProviders { get; }
        IPropertyEditControlRegistry PropertyEditControlProviders { get; }
        ITimerService Timer { get; }
        IDataService DataService { get; }
        IClock Clock { get; set; }
        IDashboardController DashboardController { get; set; }
        IDashboardConfigurations Configuration { get; set; }
        IRunOptions RunOptions { get; set; }
        void Register(IService service);

        T GetService<T>(string serviceName)
            where T : IService;
    }
}