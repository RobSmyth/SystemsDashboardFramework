using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.FileSystem
{
    public class FileReaderDataSourceConfigurationTileProvider : ITileControllerProvider
    {
        public FileReaderDataSourceConfigurationTileProvider(IDataSource dataSource)
        {
            
        }

        public string Name { get; }

        public bool MatchesId(string id)
        {
            throw new System.NotImplementedException();
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            throw new System.NotImplementedException();
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            throw new System.NotImplementedException();
        }
    }
}