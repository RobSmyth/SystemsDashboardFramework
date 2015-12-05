using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Registries;
using NoeticTools.SystemsDashboard.Framework.Time;
using NoeticTools.SystemsDashboard.Framework.Registries;


namespace NoeticTools.SystemsDashboard.Framework
{
    public interface IServices
    {
        KeyboardHandler KeyboardHandler { get; }
        ITileProviderRegistry TileProviders { get; }
        IPropertyEditControlRegistry PropertyEditControlProviders { get; }
        ITimerService Timer { get; }
    }
}