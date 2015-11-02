using System.Windows;
using System.Windows.Input;
using Dashboard.Tiles.Message;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;


namespace NoeticTools.Dashboard.Framework.Tiles.Message
{
    internal class MessageViewController : IViewController
    {
        private readonly IDashboardController _dashboardController;
        public static readonly string TileTypeId = "Message";
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private MessageTileControl _view;

        public MessageViewController(TileConfiguration tileConfiguration,
            IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
            _tileConfigurationConverter = new TileConfigurationConverter(tileConfiguration, this);
        }

        public ICommand ConfigureCommand { get; private set; }

        public FrameworkElement CreateView()
        {
            ConfigureCommand = new TileConfigureCommand("Message Tile Configuration", _tileConfigurationConverter, new[]
            {
                new ConfigurationParameter("Message", "X", _tileConfigurationConverter)
            },
                _dashboardController);
            _view = new MessageTileControl {DataContext = this};

            UpdateView();
            return _view;
        }

        public void OnConfigurationChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _view.message.Text = _tileConfigurationConverter.GetString("Message");
        }
    }
}