using System;
using System.Collections.Generic;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.Config.ViewModels;

namespace NoeticTools.Dashboard.Framework.Config.Commands
{
    public class TileConfigureCommand : ICommand
    {
        private readonly IConfigurationView[] _parameters;
        private readonly IDashboardController _dashboardController;
        private readonly TileConfiguration _tileConfiguration;
        private readonly string _title;
        private bool _canExecute = true;

        public TileConfigureCommand(string title, TileConfiguration tileConfiguration, IConfigurationView[] parameters, IDashboardController dashboardController)
        {
            _title = title;
            _tileConfiguration = tileConfiguration;
            _parameters = parameters;
            _dashboardController = dashboardController;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is bool)
            {
                _canExecute = (bool) parameter;
                OnChanExecuteChanged();
            }
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            var viewModel = new TileConfigationViewModel(_title, _tileConfiguration, _parameters);
            _dashboardController.ShowOnSidePane(viewModel);
        }

        public event EventHandler CanExecuteChanged;

        private void OnChanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }
    }
}