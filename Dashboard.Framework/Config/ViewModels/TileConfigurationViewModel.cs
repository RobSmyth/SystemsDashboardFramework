using System.Collections.Generic;
using System.Windows;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.Config.Views;

namespace NoeticTools.Dashboard.Framework.Config.ViewModels
{
    internal class TileConfigationViewModel
    {
        private readonly IEnumerable<IConfigurationView> _parameters;
        private readonly TileConfiguration _tileConfiguration;
        private readonly string _title;
        private TileConfigurationView _view;

        public TileConfigationViewModel(string title, TileConfiguration tileConfiguration,
            IEnumerable<IConfigurationView> parameters)
        {
            _title = title;
            _tileConfiguration = tileConfiguration;
            _parameters = parameters;
        }

        public void Show()
        {
            _view = new TileConfigurationView {DataContext = this, Title = _title};
            _view.OkButton.Click += OkButton_Click;
            _view.CancelButton.Click += CancelButton_Click;

            foreach (var parameter in _parameters)
            {
                parameter.Show(_view.ParametersGrid, _tileConfiguration);
            }

            _view.Show();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _view.Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var parameter in _parameters)
            {
                parameter.Save(_view.ParametersGrid);
            }

            _view.Close();
        }
    }
}