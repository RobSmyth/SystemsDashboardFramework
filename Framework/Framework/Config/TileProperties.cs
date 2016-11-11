using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public sealed class TileProperties : ITileProperties
    {
        public TileProperties(IItemConfiguration tileConfiguration, IConfigurationChangeListener listener, IServices services)
        {
            Properties = new TileConfigurationConverter(tileConfiguration, listener);
            NamedValueRepository = new ConfigurationNamedValueRepositoryDecorator(Properties, new NamedValueRepositoryAggregator(new DataSourceNamedValueRepositoryProvider(services), new NullNamedValueRepositoryProvider()));
        }

        public INamedValueRepository Properties { get; }
        public INamedValueRepository NamedValueRepository { get; }
    }
}