using System.Collections.Generic;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public interface IProject
    {
        string Name { get; }
        Project Inner { get; }
    }
}