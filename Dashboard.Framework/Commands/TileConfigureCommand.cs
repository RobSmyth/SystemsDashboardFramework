using System;
using System.Collections.Generic;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Config.Controllers;


namespace NoeticTools.SystemsDashboard.Framework.Commands
{
    public class TileConfigureCommand : ICommand
    {
        private readonly IPropertyViewModel[] _parameters;
        private readonly IDashboardController _dashboardController;
        private readonly TileLayoutController _tileLayoutController;
        private readonly IServices _services;
        private readonly TileConfiguration _tile;
        private readonly string _title;
        private bool _canExecute = true;

        public TileConfigureCommand(TileConfiguration tile, string title, IPropertyViewModel[] parameters, IDashboardController dashboardController, TileLayoutController tileLayoutController, IServices services)
        {
            _tile = tile;
            _title = title;
            _parameters = new List<IPropertyViewModel>(parameters)
            {
                new DividerPropertyViewModel(),
                new TileRowSpanViewModel(tile),
                new TileColumnSpanViewModel(tile)
            }.ToArray();
            _dashboardController = dashboardController;
            _tileLayoutController = tileLayoutController;
            _services = services;
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
            var routedCommands = new RoutedCommands();
            routedCommands.DeleteCommandBinding.Executed += DeleteCommandBinding_Executed;
            var controller = new ConfigationViewController(_title, routedCommands, _parameters, _services);
            _dashboardController.ShowOnSidePane(controller.CreateView(), _title);
        }

        private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _tileLayoutController.Remove(_tile);
        }

        private void OnChanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            handler?.Invoke(this, new EventArgs());
        }

        public event EventHandler CanExecuteChanged;
    }
}