using System.Collections.Generic;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Input;
using NoeticTools.TeamStatusBoard.Framework.Registries;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.Framework.Styles;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Services
{
    public interface IServices
    {
        KeyboardHandler KeyboardHandler { get; }
        ITileProviderRegistry TileProviders { get; }
        IPropertyEditControlRegistry PropertyEditControlProviders { get; }
        ITimerService Timer { get; }
        IDataService DataService { get; }
        IClock Clock { get; }
        IDashboardController DashboardController { get; }
        IDashboardConfigurations Configuration { get; }
        IRunOptions RunOptions { get; }
        void Register(IService service);

        T GetService<T>(string serviceName)
            where T : IService;

        IStatusBoardStyle Style { get; }

        IEnumerable<T> GetServicesOfType<T>()
            where T : class, IService;
    }
}