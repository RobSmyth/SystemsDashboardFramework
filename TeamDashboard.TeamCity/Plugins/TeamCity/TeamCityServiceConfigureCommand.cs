using System;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public class TeamCityServiceConfigureCommand : ICommand
    {
        private readonly ITeamCityChannel _service;

        public TeamCityServiceConfigureCommand(ITeamCityChannel service)
        {
            _service = service;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _service.Configure();
        }

        public event EventHandler CanExecuteChanged;
    }
}