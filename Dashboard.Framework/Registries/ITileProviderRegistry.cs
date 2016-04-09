using System.Collections.Generic;
using NoeticTools.SystemsDashboard.Framework;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public interface ITileProviderRegistry
    {
        IEnumerable<ITileControllerProvider> GetAll();
        void Register(ITileControllerProvider provider);
    }
}