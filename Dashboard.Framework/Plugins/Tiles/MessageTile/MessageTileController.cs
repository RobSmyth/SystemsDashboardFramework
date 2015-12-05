﻿using System.Windows;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.MessageTile;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.MessageTile
{
    internal sealed class MessageTileController : IViewController
    {
        private readonly TileConfiguration _tile;
        private readonly IDashboardController _dashboardController;
        public static readonly string TileTypeId = "Message";
        private readonly TileLayoutController _layoutController;
        private readonly IServices _services;

        public MessageTileController(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services)
        {
            _tile = tile;
            _dashboardController = dashboardController;
            _layoutController = layoutController;
            _services = services;
        }

        public FrameworkElement CreateView()
        {
            return new MessageTileControl {DataContext = new MessageViewModel(_tile, _dashboardController, _layoutController, _services)};
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}