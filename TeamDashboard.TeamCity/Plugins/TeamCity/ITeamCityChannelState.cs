namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    internal interface ITeamCityChannelState : ITeamCityChannel
    {
        void Leave();
        void Enter();
    }
}