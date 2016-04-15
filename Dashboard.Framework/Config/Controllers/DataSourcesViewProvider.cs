using System.Windows;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Config.Views;
using NoeticTools.TeamStatusBoard.Framework.Services;
using DataSourcesConfigControl = NoeticTools.TeamStatusBoard.Framework.Config.Views.DataSourcesConfigControl;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Controllers
{
    public sealed class DataSourcesViewProvider
    {
        private readonly TsbCommands _commands;
        private readonly IServices _services;
        private PaneWithTitleBarControl _panelView;

        public DataSourcesViewProvider(TsbCommands commands, IServices services)
        {
            _commands = commands;
            _services = services;
        }

        public FrameworkElement CreateView()
        {
            _commands.CloseCommandBinding.Executed += CloseCommandBinding_Executed;

            var view = new DataSourcesConfigControl(_commands) { DataContext = new DataSourcesViewModel(_services) };

            _panelView = new PaneWithTitleBarControl("Data sources", view, _commands)
            {
                Width = double.NaN,
                Height = double.NaN
            };

            return _panelView;
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _panelView.Visibility = Visibility.Collapsed;
        }
    }
}