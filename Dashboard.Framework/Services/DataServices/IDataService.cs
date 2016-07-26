using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public interface IDataService : IService, IDataSource
    {
        void Register(string name, IDataSource dataSource);
        IEnumerable<IDataSource> GetAllDataSources();
        IDataSource Get(string name);
    }
}