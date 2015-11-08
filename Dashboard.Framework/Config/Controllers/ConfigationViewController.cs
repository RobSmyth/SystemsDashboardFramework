using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.ViewModels;
using NoeticTools.Dashboard.Framework.Config.Views;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Config.Controllers
{
    public class ConfigationViewController : NotifyingViewModelBase, IViewController
    {
        private readonly IEnumerable<IConfigurationElement> _parameters;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly string _title;
        private ParametersConfigControl _view;
        private PaneWithTitleBarControl _panelView;

        public ConfigationViewController(string title, TileConfigurationConverter tileConfigurationConverter, IConfigurationElement[] parameters)
        {
            _title = title;
            _tileConfigurationConverter = tileConfigurationConverter;
            _parameters = parameters;
        }

        public FrameworkElement CreateView()
        {
            var commands = new RoutedCommands();
            commands.CloseCommandBinding.Executed += CloseCommandBinding_Executed;

            _view = new ParametersConfigControl(commands, _parameters, _tileConfigurationConverter) {DataContext = this};

            _panelView = new PaneWithTitleBarControl(new PanelWithTitleBarViewModel(_title), _view)
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

        public void OnConfigurationChanged()
        {
        }
    }
}