namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel
{
    internal interface ITeamCityChannelState : ITeamCityIoChannel
    {
        void Leave();
        void Enter();
    }
}