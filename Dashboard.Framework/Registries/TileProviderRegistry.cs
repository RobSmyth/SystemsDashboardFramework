using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public class TileProviderRegistry : ITileProviderRegistry
    {
        private readonly IList<ITileControllerProvider> _providers = new List<ITileControllerProvider>();

        public IEnumerable<ITileControllerProvider> GetAll()
        {
            return _providers.ToArray();
        }

        public void Register(ITileControllerProvider provider)
        {
            _providers.Add(provider);
        }
    }
}