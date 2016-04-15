using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.TeamCity
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