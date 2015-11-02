using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Views;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Config.ViewModels
{
    internal class ConfigationViewController : NotifyingViewModelBase, IViewController
    {
        private readonly IEnumerable<IConfigurationView> _parameters;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly string _title;
        private ParametersConfigControl _view;

        public ConfigationViewController(string title, TileConfigurationConverter tileConfigurationConverter, IConfigurationView[] parameters)
        {
            _title = title;
            _tileConfigurationConverter = tileConfigurationConverter;
            _parameters = parameters;
            SaveCommand = new NullCommand();
            CloseCommand = new NullCommand();
        }

        public ICommand CloseCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public FrameworkElement CreateView()
        {
            _view = new ParametersConfigControl();

            foreach (var parameter in _parameters)
            {
                parameter.Show(_view.PlaceholderGrid, _tileConfigurationConverter);
            }

            CloseCommand = new CloseCommand(_view);
            SaveCommand = new SaveConfigurationParametersCommand(_parameters, _view.PlaceholderGrid);

            _view.DataContext = this;

            return _view;
        }

        public void OnConfigurationChanged()
        {
        }
    }
}