using System;
using System.Collections.Generic;
using TeamCitySharp.ActionTypes;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop
{
    public class TxSharpAgents : IAgents
    {
        private readonly IAgents _inner;

        public TxSharpAgents(IAgents inner)
        {
            _inner = inner;
        }

        public List<Agent> All()
        {
            return SafeInvoke(() => _inner.All());
        }

        public List<Agent> AllConnected()
        {
            return SafeInvoke(() => _inner.AllConnected());
        }

        public List<Agent> AllAuthorised()
        {
            return SafeInvoke(() => _inner.AllAuthorised());
        }

        private List<Agent> SafeInvoke(Func<List<Agent>> action)
        {
            try
            {
                return action();
            }
            catch (Exception)
            {
                return new List<Agent>();
            }
        }
    }
}