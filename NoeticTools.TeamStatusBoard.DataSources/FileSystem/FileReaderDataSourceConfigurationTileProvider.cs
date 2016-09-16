using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.DataSources.FileSystem
{
    public class FileReaderDataSourceConfigurationTileProvider : ITileControllerProvider
    {
        public FileReaderDataSourceConfigurationTileProvider(IDataSource dataSource)
        {
            
        }

        public string Name { get; }

        public string TypeId => "";

        public bool MatchesId(string id)
        {
            throw new System.NotImplementedException();
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            throw new System.NotImplementedException();
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            throw new System.NotImplementedException();
        }
    }
}