

namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TcSharpInterop
{
    public sealed class NullInteropProject : TeamCitySharp.DomainEntities.Project
    {
        public NullInteropProject(string name, string description = "Unknown", string id= "")
        {
            Name = name;
            Description = description;
            Id = id;
        }
    }
}