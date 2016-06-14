namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel
{
    internal interface ITeamCityChannelState : ITeamCityIoChannel
    {
        void Leave();
        void Enter();
    }
}