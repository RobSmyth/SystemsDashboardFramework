using System.Collections.Generic;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public interface ITileProviderRegistry
    {
        IEnumerable<ITileControllerProvider> GetAll();
        void Register(ITileControllerProvider provider);
    }
}