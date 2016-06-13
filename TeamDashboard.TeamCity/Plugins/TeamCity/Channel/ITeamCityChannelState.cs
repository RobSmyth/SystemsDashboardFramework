namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Channel
{
    internal interface ITeamCityChannelState : ITeamCityIoChannel
    {
        void Leave();
        void Enter();
    }
}