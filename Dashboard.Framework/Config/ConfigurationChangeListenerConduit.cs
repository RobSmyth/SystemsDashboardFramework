using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public sealed class ConfigurationChangeListenerConduit : IConfigurationChangeListener
    {
        private IConfigurationChangeListener _listener;

        public void OnConfigurationChanged(INamedValueRepository converter)
        {
            _listener.OnConfigurationChanged(converter);
        }

        public void SetTarget(IConfigurationChangeListener listener)
        {
            _listener = listener;
        }
    }
}