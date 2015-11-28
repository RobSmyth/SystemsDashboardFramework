﻿using System.Windows;
using Dashboard.Tiles.Message;
using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles.Message
{
    internal class MessageTileController : IViewController
    {
        private readonly TileConfiguration _tileConfiguration;
        private readonly IDashboardController _dashboardController;
        public static readonly string TileTypeId = "Message";
        private MessageTileControl _view;

        public MessageTileController(TileConfiguration tileConfiguration,
            IDashboardController dashboardController)
        {
            _tileConfiguration = tileConfiguration;
            _dashboardController = dashboardController;
        }

        public FrameworkElement CreateView()
        {
            _view = new MessageTileControl();
            _view.DataContext = new MessageViewModel(_tileConfiguration, _dashboardController);

            return _view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}