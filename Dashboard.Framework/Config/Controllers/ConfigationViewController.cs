using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config.Views;


namespace NoeticTools.SystemsDashboard.Framework.Config.Controllers
{
    public sealed class ConfigationViewController : NotifyingViewModelBase
    {
        private readonly IEnumerable<IPropertyViewModel> _parameters;
        private readonly IServices _services;
        private readonly string _title;
        private readonly RoutedCommands _routedCommands;
        private PaneWithTitleBarControl _panelView;

        public ConfigationViewController(string title, RoutedCommands routedCommands, IPropertyViewModel[] parameters, IServices services)
        {
            _title = title;
            _routedCommands = routedCommands;
            _parameters = parameters;
            _services = services;
        }

        public FrameworkElement CreateView()
        {
            _routedCommands.CloseCommandBinding.Executed += CloseCommandBinding_Executed;

            var view = new ParametersConfigControl(_routedCommands, _parameters, _services) {DataContext = this};

            _panelView = new PaneWithTitleBarControl(_title, view, _routedCommands)
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