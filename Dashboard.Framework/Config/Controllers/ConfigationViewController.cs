using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config.Views;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Config.Controllers
{
    public class ConfigationViewController : NotifyingViewModelBase, IViewController
    {
        private readonly IEnumerable<IElementViewModel> _parameters;
        private readonly string _title;
        private PaneWithTitleBarControl _panelView;

        public ConfigationViewController(string title, IElementViewModel[] parameters)
        {
            _title = title;
            _parameters = parameters;
        }

        public FrameworkElement CreateView()
        {
            var commands = new RoutedCommands();
            commands.CloseCommandBinding.Executed += CloseCommandBinding_Executed;

            var view = new ParametersConfigControl(commands, _parameters) {DataContext = this};

            _panelView = new PaneWithTitleBarControl(_title, view, commands)
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

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}