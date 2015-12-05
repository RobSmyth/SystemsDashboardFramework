using System.Collections.Generic;


namespace NoeticTools.SystemsDashboard.Framework.Registries
{
    public interface ITileProviderRegistry
    {
        IEnumerable<ITileControllerProvider> GetAll();
        void Register(ITileControllerProvider provider);
    }
}