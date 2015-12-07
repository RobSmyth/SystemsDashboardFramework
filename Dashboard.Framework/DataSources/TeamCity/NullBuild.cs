using PropertyTools.DataAnnotations;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    internal class NullBuild : Build
    {
        public NullBuild() : base()
        {
            this.Id = "";
            this.StatusText = "X";
            Status = "X";
            Number = "1.2.3-1234";
        }
    }
}