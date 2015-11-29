using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config.Views;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Config.Controllers
{
    public sealed class ConfigationViewController : NotifyingViewModelBase, IViewController
    {
        private readonly IEnumerable<IElementViewModel> _parameters;
        private readonly string _title;
        private readonly RoutedCommands _routedCommands;
        private PaneWithTitleBarControl _panelView;

        public ConfigationViewController(string title, RoutedCommands routedCommands, IElementViewModel[] parameters)
        {
            _title = title;
            _routedCommands = routedCommands;
            _parameters = parameters;
        }

        public FrameworkElement CreateView()
        {
            _routedCommands.CloseCommandBinding.Executed += CloseCommandBinding_Executed;

            var view = new ParametersConfigControl(_routedCommands, _parameters) {DataContext = this};

            _panelView = new PaneWithTitleBarControl(_title, view, _routedCommands)
            {
                Width = double.NaN,
                Height = double.NaN
            };

            return _panelView;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _panelView.Visibility = Visibility.Collapsed;
        }
    }
}