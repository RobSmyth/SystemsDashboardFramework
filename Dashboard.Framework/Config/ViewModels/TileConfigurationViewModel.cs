using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Annotations;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.Config.Views;
using NoeticTools.Dashboard.Framework.Panes;

namespace NoeticTools.Dashboard.Framework.Config.ViewModels
{
    internal class TileConfigationViewModel : NotifyingViewModelBase, IPaneViewModel
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
            Save = new NullCommand();
            Close = new NullCommand();
        }

        public ICommand Close { get; private set; }
        public ICommand Save { get; private set; }

        public UserControl Show()
        {
            _view = new TileConfigurationControl {Title = {Text = _title}};

            foreach (var parameter in _parameters)
            {
                parameter.Show(_view.ParametersGrid, _tileConfiguration);
            }

            Close = new CloseCommand(_view);
            Save = new SaveConfigurationParametersCommand(_parameters, _view.ParametersGrid);

            _view.DataContext = this;

            return _view;
        }
    }
}