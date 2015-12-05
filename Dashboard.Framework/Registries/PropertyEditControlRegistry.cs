using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public class PropertyEditControlRegistry : IPropertyEditControlRegistry
    {
        private readonly IList<IPropertyViewProvider>  _providers = new List<IPropertyViewProvider>();

        public void Register(IPropertyViewProvider provider)
        {
            _providers.Add(provider);
        }

        public IPropertyViewProvider Get(string elementType)
        {
            return _providers.First(x => x.CanHandle(elementType));
        }
    }
}