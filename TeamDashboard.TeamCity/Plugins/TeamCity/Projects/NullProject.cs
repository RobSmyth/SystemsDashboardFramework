using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public class NullProject : IProject
    {
        public string Name { get; }
        public Project Inner { get; }

        public NullProject(string name)
        {
            Name = name;
            Inner = new NullTcSharpProject();
        }
    }
}