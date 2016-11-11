namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel
{
    internal interface ITeamCityChannelState : ITeamCityIoChannel
    {
        void Leave();
        void Enter();
    }
}