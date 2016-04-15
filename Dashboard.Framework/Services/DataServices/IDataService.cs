using System.Collections.Generic;
using NoeticTools.SystemsDashboard.Framework.Services;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public interface IDataService : IService, IDataSource
    {
        IDataSource GetDataSource(string name);
        void Register(string name, IDataSource dataSource);
        IEnumerable<IDataSource> GetAllDataSources();
    }
}