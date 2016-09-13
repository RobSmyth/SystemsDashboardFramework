using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public class TileProperties
    {
        public TileProperties(IItemConfiguration tileConfiguration, IConfigurationChangeListener listener, IServices services)
        {
            Properties = new TileConfigurationConverter(tileConfiguration, listener);
            NamedValueReader = new ConfigurationNamedValueReaderDecorator(Properties, new NamedValueReaderAggregator(new DataSourceNamedValueReaderProvider(services), new NullNamedValueReaderProvider()));
        }

        public INamedValueReader Properties { get; }
        public INamedValueReader NamedValueReader { get; }
    }
}