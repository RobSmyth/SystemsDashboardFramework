namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop
{
    public sealed class NullInteropBuildConfig : TeamCitySharp.DomainEntities.BuildConfig
    {
        public NullInteropBuildConfig(string name)
        {
            Name = name;
            Description = "Unknown";
            Id = "";
        }
    }
}