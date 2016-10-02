using System;
using System.Windows.Input;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.ColouredTile
{
    public class ColouredTileViewModel : ConfiguredTileViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private readonly INamedValueRepository _tileConfigurationConverter;
        private Brush _background;

        public ColouredTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, ColouredTileControl view, ITileProperties properties)
            : base(properties)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[] {new PropertyViewModel("Colour", PropertyType.Text, _tileConfigurationConverter, new ColourSuggestionsProvider(services))};
            ConfigureCommand = new TileConfigureCommand(tile, "Coloured Tile Configuration", parameters, dashboardController, layoutController, services);
            view.DataContext = this;
            Update();
        }

        public Brush Background
        {
            get { return _background; }
            private set
            {
                if (value.Equals(_background)) return;
                _background = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(INamedValueRepository converter)
        {
            Update();
        }

        private void Update()
        {
            try
            {
                var value = _tileConfigurationConverter.GetString("Colour", "Gray");
                Background = new SolidColorBrush((Color) ColorConverter.ConvertFromString(value));
            }
            catch (Exception)
            {
                Background = Brushes.Red;
                _tileConfigurationConverter.SetParameter("Colour", "Red");
            }
        }
    }
}