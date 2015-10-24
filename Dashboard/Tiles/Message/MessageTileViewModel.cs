using System;
using System.Windows.Controls;
using System.Windows.Input;
using Dashboard.Config;
using Dashboard.Config.Parameters;
using Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.TeamDashboard.Tiles;

namespace Dashboard.Tiles.Message
{
    internal class MessageTileViewModel : ITileViewModel
    {
        public static readonly string TileTypeId = "Message";
        private readonly TileConfiguration _tileConfiguration;
        private MessageTileControl _view;

        public MessageTileViewModel(DashboardTileConfiguration tileConfiguration)
        {
            _tileConfiguration = new TileConfiguration(tileConfiguration, this);
            Id = tileConfiguration.Id;
        }

        public ICommand ConfigureCommand { get; private set; }

        public void Start(Panel placeholderPanel)
        {
            ConfigureCommand = new TileConfigureCommand("Message Tile Configuration", _tileConfiguration, new[]
            {
                new ConfigurationParameter("Message", "X", _tileConfiguration)
            });
            _view = new MessageTileControl {DataContext = this};
            placeholderPanel.Children.Add(_view);

            UpdateView();
        }

        public string TypeId
        {
            get { return TileTypeId; }
        }

        public Guid Id { get; private set; }

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