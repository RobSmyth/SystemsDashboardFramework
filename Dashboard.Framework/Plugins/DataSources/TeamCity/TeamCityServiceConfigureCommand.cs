﻿using System;
using System.Windows.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.TeamCity
{
    public class TeamCityServiceConfigureCommand : ICommand
    {
        private readonly TeamCityService _service;

        public TeamCityServiceConfigureCommand(TeamCityService service)
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