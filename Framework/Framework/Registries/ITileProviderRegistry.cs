using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public interface ITileProviderRegistry
    {
        IEnumerable<ITileControllerProvider> GetAll();
        void Register(ITileControllerProvider provider);
    }
}