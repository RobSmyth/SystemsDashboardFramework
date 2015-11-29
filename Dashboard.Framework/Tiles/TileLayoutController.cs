﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using NoeticTools.Dashboard.Framework.Adorners;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Registries;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public class TileLayoutController : ITileLayoutController
    {
        private readonly Thickness _normalMargin;
        private readonly TileDragAndDropController _dragAndDropController;
        private TileConfiguration _tileConfiguration;
        private readonly Grid _tileGrid;
        private readonly ITileLayoutControllerRegistry _tileLayoutControllerRegistry;
        private readonly ITileControllerFactory _tileFactory;
        private readonly IList<TileConfiguration> _tileConfigurations = new List<TileConfiguration>();

        public TileLayoutController(Grid tileGrid, ITileControllerFactory tileFactory, ITileLayoutControllerRegistry tileLayoutControllerRegistry, Thickness normalMargin, TileDragAndDropController dragAndDropController)
        {
            _tileFactory = tileFactory;
            _tileLayoutControllerRegistry = tileLayoutControllerRegistry;
            _normalMargin = normalMargin;
            _dragAndDropController = dragAndDropController;
            _tileGrid = tileGrid;
            _tileGrid.Margin = _normalMargin;
        }

        public void Load(TileConfiguration tileConfiguration)
        {
            Clear();
            _tileConfiguration = tileConfiguration;
            foreach (TileConfiguration childTile in _tileConfiguration.Tiles)
            {
                AddTile(childTile);
            }
        }

        public void AddTile(TileConfiguration tileConfiguration)
        {
            _tileConfigurations.Add(tileConfiguration);

            ITileLayoutController layoutController = null;

            if (tileConfiguration.TypeId == TileConfiguration.BlankTileTypeId || tileConfiguration.TypeId.Equals("6f1bf918-6080-42c2-b980-d562f77cb4e6",
                StringComparison.InvariantCultureIgnoreCase))
            {
                tileConfiguration.TypeId = TileConfiguration.BlankTileTypeId;
                layoutController = AddPanel(tileConfiguration);
            }
            else
            {
                layoutController = AddTile(tileConfiguration, _tileFactory.Create(tileConfiguration));
            }

            //layoutController.AddTiles(tileConfiguration.Tiles);
        }

        public void Clear()
        {
            _tileGrid.Children.Clear();
            _tileGrid.RowDefinitions.Clear();
            _tileGrid.ColumnDefinitions.Clear();
            _tileConfigurations.Clear();
        }

        public void ToggleShowGroupPanelDetailsMode()
        {
            if (_tileGrid.Margin == _normalMargin)
            {
                var groupTileHighlightAdorner = new GroupPanelDetailsAdorner(_tileGrid);
                groupTileHighlightAdorner.Attach();
            }
            else
            {
                _tileGrid.Margin = _normalMargin;
                var layer = AdornerLayer.GetAdornerLayer(_tileGrid);
                var tileAdorners = layer.GetAdorners(_tileGrid).Where(x => x is GroupPanelDetailsAdorner).Cast<GroupPanelDetailsAdorner>().ToArray();
                foreach (var tileAdorner in tileAdorners)
                {
                    tileAdorner.Detach();
                }
            }
        }

        public void InsertNewRow(int insertAtRowNumber)
        {
            var existingRowsCount = _tileGrid.RowDefinitions.Count;
            AddRowsToFit((insertAtRowNumber > existingRowsCount) ? insertAtRowNumber : existingRowsCount + 1, 1);

            foreach (var tileConfiguration in _tileConfigurations.Where(tileConfiguration => tileConfiguration.RowNumber >= insertAtRowNumber))
            {
                tileConfiguration.RowNumber++;
            }

            foreach (UIElement child in _tileGrid.Children)
            {
                var rowIndex = Grid.GetRow(child);
                if (rowIndex >= insertAtRowNumber-1)
                {
                    Grid.SetRow(child, rowIndex+1);
                }
            }
        }

        public void InsertNewColumn(int insertAtColumnNumber)
        {
            var existingColumnsCount = _tileGrid.ColumnDefinitions.Count;
            AddColumnsToFit((insertAtColumnNumber > existingColumnsCount) ? insertAtColumnNumber : existingColumnsCount + 1, 1);

            foreach (var tileConfiguration in _tileConfigurations.Where(tileConfiguration => tileConfiguration.ColumnNumber >= insertAtColumnNumber))
            {
                tileConfiguration.ColumnNumber++;
            }

            foreach (UIElement child in _tileGrid.Children)
            {
                var columnIndex = Grid.GetColumn(child);
                if (columnIndex >= insertAtColumnNumber-1)
                {
                    Grid.SetColumn(child, columnIndex + 1);
                }
            }
        }

        private TileLayoutController AddTile(TileConfiguration tileConfiguration, IViewController viewController)
        {
            var panel = AddPlaceholderPanel(tileConfiguration.RowNumber, tileConfiguration.ColumnNumber, tileConfiguration.RowSpan, tileConfiguration.ColumnSpan);
            panel.Margin = new Thickness(2);
            var view = viewController.CreateView();
            panel.Children.Add(view);

            _dragAndDropController.RegisterTarget(view, this, tileConfiguration);

            return this;
        }

        private ITileLayoutController AddPanel(TileConfiguration tileConfiguration)
        {
            var panel = AddPlaceholderPanel(tileConfiguration.RowNumber, tileConfiguration.ColumnNumber, tileConfiguration.RowSpan, tileConfiguration.ColumnSpan);
            return _tileLayoutControllerRegistry.GetNew(panel, tileConfiguration);
        }

        private Grid AddPlaceholderPanel(int rowNumber, int columnNumber, int rowSpan, int columnSpan)
        {
            AddRowsToFit(rowNumber, rowSpan);
            AddColumnsToFit(columnNumber, columnSpan);

            var groupPanel = new Grid {Name = $"Panel{_tileLayoutControllerRegistry.Count + 1}"};

            Grid.SetRow(groupPanel, rowNumber - 1);
            Grid.SetColumn(groupPanel, columnNumber - 1);
            Grid.SetRowSpan(groupPanel, rowSpan);
            Grid.SetColumnSpan(groupPanel, columnSpan);

            _tileGrid.Children.Add(groupPanel);
            return groupPanel;
        }

        private void AddColumnsToFit(int columnNumber, int columnSpan)
        {
            while (columnNumber + columnSpan - 1 > _tileGrid.ColumnDefinitions.Count)
            {
                _tileGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void AddRowsToFit(int rowNumber, int rowSpan)
        {
            while (rowNumber + rowSpan - 1 > _tileGrid.RowDefinitions.Count)
            {
                _tileGrid.RowDefinitions.Add(new RowDefinition());
            }
        }
    }
}