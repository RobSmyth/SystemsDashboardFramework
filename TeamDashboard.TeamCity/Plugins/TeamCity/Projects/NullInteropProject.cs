

namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public sealed class NullInteropProject : TeamCitySharp.DomainEntities.Project
    {
        public NullInteropProject(string name)
        {
            Name = name;
            Description = "Unknown";
            Id = "";
        }
    }
}