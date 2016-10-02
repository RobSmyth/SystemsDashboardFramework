using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public interface IConfigurationChangeListener
    {
        void OnConfigurationChanged(INamedValueRepository converter);
    }
}