using System.Windows;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config.Views;
using NoeticTools.SystemsDashboard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Services;
using DataSourcesConfigControl = NoeticTools.TeamStatusBoard.Framework.Config.Views.DataSourcesConfigControl;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Controllers
{
    public sealed class DataSourcesViewController : NotifyingViewModelBase
    {
        private readonly IServices _services;
        private readonly string _title;
        private readonly ApplicationCommandsBindings _applicationCommandsBindings;
        private PaneWithTitleBarControl _panelView;

        public DataSourcesViewController(string title, ApplicationCommandsBindings applicationCommandsBindings, IServices services)
        {
            _title = title;
            _applicationCommandsBindings = applicationCommandsBindings;
            _services = services;
        }

        public FrameworkElement CreateView()
        {
            //_applicationCommandsBindings.CloseCommandBinding.Executed += CloseCommandBinding_Executed;

            var view = new DataSourcesConfigControl(_applicationCommandsBindings, _services) { DataContext = this };

            _panelView = new PaneWithTitleBarControl(_title, view, _applicationCommandsBindings)
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