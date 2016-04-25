using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public interface IDataService : IService, IDataSource
    {
        IDataSource GetDataSource(string typeName, string name);
        void Register(string name, IDataSource dataSource, ITileControllerProvider configurationTileProvider);
        IEnumerable<IDataSource> GetAllDataSources();
    }
}