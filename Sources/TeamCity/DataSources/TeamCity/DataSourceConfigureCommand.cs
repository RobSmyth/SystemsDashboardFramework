using System;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity
{
    public class DataSourceConfigureCommand : ICommand
    {
        private readonly ITeamCityChannel _service;

        public DataSourceConfigureCommand(ITeamCityChannel service)
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