namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel
{
    public interface ITeamCityChannel : ITeamCityIoChannel
    {
        void Stop();
        void Start();
        void Configure();
    }
}