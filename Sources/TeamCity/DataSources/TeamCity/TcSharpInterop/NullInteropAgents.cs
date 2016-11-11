using System.Collections.Generic;
using TeamCitySharp.ActionTypes;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TcSharpInterop
{
    public sealed class NullInteropAgents : IAgents
    {
        public List<Agent> All()
        {
            return new List<Agent>();
        }

        public List<Agent> AllConnected()
        {
            return new List<Agent>();
        }

        public List<Agent> AllAuthorised()
        {
            return new List<Agent>();
        }
    }
}
