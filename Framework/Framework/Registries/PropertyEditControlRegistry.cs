using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public class PropertyEditControlRegistry : IPropertyEditControlRegistry
    {
        private readonly IList<IPropertyViewProvider> _providers = new List<IPropertyViewProvider>();

        public void Register(IPropertyViewProvider provider)
        {
            _providers.Add(provider);
        }

        public IPropertyViewProvider Get(PropertyType elementType)
        {
            return _providers.First(x => x.CanHandle(elementType));
        }
    }
}