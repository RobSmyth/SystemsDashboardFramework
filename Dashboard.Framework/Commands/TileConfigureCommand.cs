using System;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Controllers;


namespace NoeticTools.Dashboard.Framework.Commands
{
    public class TileConfigureCommand : ICommand
    {
        private readonly IElementViewModel[] _parameters;
        private readonly IDashboardController _dashboardController;
        private readonly TileLayoutController _tileLayoutController;
        private readonly TileConfiguration _tile;
        private readonly string _title;
        private bool _canExecute = true;

        public TileConfigureCommand(TileConfiguration tile, string title, IElementViewModel[] parameters, IDashboardController dashboardController, TileLayoutController tileLayoutController)
        {
            _tile = tile;
            _title = title;
            _parameters = parameters;
            _dashboardController = dashboardController;
            _tileLayoutController = tileLayoutController;
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
            var controller = new ConfigationViewController(_title, routedCommands, _parameters);
            _dashboardController.ShowOnSidePane(controller, _title);
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