

namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop
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