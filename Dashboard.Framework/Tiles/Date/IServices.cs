using NoeticTools.Dashboard.Framework.Registries;


namespace NoeticTools.Dashboard.Framework.Tiles.Date
{
    public interface IServices
    {
        ITileControllerRegistry Repository { get; }
        KeyboardHandler KeyboardHandler { get; }
    }
}