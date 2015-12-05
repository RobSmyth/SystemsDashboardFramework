namespace NoeticTools.SystemsDashboard.Framework.Config
{
    public interface IConfigurationChangeListener
    {
        void OnConfigurationChanged(TileConfigurationConverter converter);
    }
}