using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Builds
{
    internal class NullBuild : Build
    {
        public NullBuild()
        {
            Id = "";
            StatusText = "X";
            Status = "X";
            Number = "1.2.3-1234";
        }
    }
}