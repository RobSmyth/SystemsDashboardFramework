namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public interface IDashboardConfiguration
    {
        string Name { get; set; }
        string DisplayName { get; set; }
        TileConfiguration RootTile { get; set; }
    }
}