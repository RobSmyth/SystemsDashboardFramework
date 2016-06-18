using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCitySharp.ActionTypes;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop
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
