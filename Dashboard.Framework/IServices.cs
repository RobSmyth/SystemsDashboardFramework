using NoeticTools.Dashboard.Framework.Input;
using NoeticTools.Dashboard.Framework.Registries;


namespace NoeticTools.Dashboard.Framework
{
    public interface IServices
    {
        ITileControllerFactory TileControllerRepository { get; }
        KeyboardHandler KeyboardHandler { get; }
        ITileProviderRegistry TileProviderRegistry { get; }
    }
}