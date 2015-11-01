using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.Config.Views;
using NoeticTools.Dashboard.Framework.Tiles;

namespace NoeticTools.Dashboard.Framework.Config.ViewModels
{
    internal class TileConfigationViewModel : NotifyingViewModelBase, ITileViewModel
    {
        private readonly IEnumerable<IConfigurationView> _parameters;
        private readonly TileConfiguration _tileConfiguration;
        private readonly string _title;
        private TileConfigurationControl _view;

        public TileConfigationViewModel(string title, TileConfiguration tileConfiguration, IConfigurationView[] parameters)
        {
            _title = title;
            _tileConfiguration = tileConfiguration;
            _parameters = parameters;
            SaveCommand = new NullCommand();
            CloseCommand = new NullCommand();
            ConfigureCommand = new NullCommand();
        }

        public ICommand CloseCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand ConfigureCommand { get; }

        public FrameworkElement CreateView()
        {
            _view = new TileConfigurationControl {Title = {Text = _title}};

            foreach (var parameter in _parameters)
            {
                parameter.Show(_view.ParametersGrid, _tileConfiguration);
            }

            CloseCommand = new CloseCommand(_view);
            SaveCommand = new SaveConfigurationParametersCommand(_parameters, _view.ParametersGrid);

            _view.DataContext = this;

            return _view;
        }

        public void OnConfigurationChanged()
        {
        }
    }
}