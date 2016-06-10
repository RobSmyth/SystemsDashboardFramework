using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    internal class NullTcSharpProject : Project
    {
        public NullTcSharpProject()
        {
            Id = "";
        }
    }
}