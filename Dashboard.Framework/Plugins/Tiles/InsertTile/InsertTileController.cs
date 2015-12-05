﻿using System.Collections.Generic;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Registries;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.InsertTile
{
    internal sealed class InsertTileController : IViewController
    {
        private readonly TileDragAndDropController _dragAndDropController;

        public InsertTileController(ITileProviderRegistry tileProviderRegistry, TileDragAndDropController dragAndDropController)
        {
            _dragAndDropController = dragAndDropController;
            TileProviders = tileProviderRegistry.GetAll();
        }

        public IEnumerable<ITileControllerProvider> TileProviders { get; set; }

        public FrameworkElement CreateView()
        {
            var view = new InsertTileControl {DataContext = this};
            _dragAndDropController.RegisterSource(view);
            return view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}