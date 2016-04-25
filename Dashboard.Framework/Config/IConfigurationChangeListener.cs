namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public interface IConfigurationChangeListener
    {
        void OnConfigurationChanged(TileConfigurationConverter converter);
    }
}