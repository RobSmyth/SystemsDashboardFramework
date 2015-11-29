using System.Collections.Generic;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public interface ITileProviderRegistry
    {
        IEnumerable<ITileControllerProvider> GetAll();
        void Register(ITileControllerProvider provider);
    }
}