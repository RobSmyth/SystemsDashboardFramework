using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public interface ITileProperties
    {
        INamedValueRepository Properties { get; }
        INamedValueRepository NamedValueRepository { get; }
    }
}