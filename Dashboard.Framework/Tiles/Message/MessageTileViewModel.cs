using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dashboard.Tiles.Message;
using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.Tiles;

namespace NoeticTools.TeamDashboard.Tiles.Message
{
    internal class MessageTileViewModel : ITileViewModel
    {
        private readonly IDashboardController _dashboardController;
        public static readonly string TileTypeId = "Message";
        private readonly TileConfiguration _tileConfiguration;
        private MessageTileControl _view;

        public MessageTileViewModel(DashboardTileConfiguration tileConfiguration, IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
            _tileConfiguration = new TileConfiguration(tileConfiguration, this);
        }

        public ICommand ConfigureCommand { get; private set; }

        public FrameworkElement CreateView()
        {
            ConfigureCommand = new TileConfigureCommand("Message Tile Configuration", _tileConfiguration, new[]
            {
                new ConfigurationParameter("Message", "X", _tileConfiguration)
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
            _view.message.Text = _tileConfiguration.GetString("Message");
        }
    }
}