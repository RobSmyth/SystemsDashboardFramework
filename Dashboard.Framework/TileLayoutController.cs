using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using NoeticTools.Dashboard.Framework.Adorners;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Input;
using NoeticTools.Dashboard.Framework.Plugins.Tiles;
using NoeticTools.Dashboard.Framework.Registries;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework
{
    public class TileLayoutController : ITileLayoutController
    {
        private readonly Thickness _normalMargin;
        private readonly TileDragAndDropController _dragAndDropController;
        private readonly Grid _tileGrid;
        private readonly ITileLayoutControllerRegistry _tileLayoutControllerRegistry;
        private readonly ITileControllerFactory _tileFactory;
        private readonly IDictionary<TileConfiguration, UIElement> _childTiles = new Dictionary<TileConfiguration, UIElement>();
        private TileConfiguration _tile;

        public TileLayoutController(Grid tileGrid, ITileControllerFactory tileFactory, ITileLayoutControllerRegistry tileLayoutControllerRegistry, Thickness normalMargin,
            TileDragAndDropController dragAndDropController)
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
            _tile = tileConfiguration;
            foreach (var childTile in _tile.Tiles)
            {
                AddTile(childTile);
            }

            RemoveEmptyRowsAndColumns();
        }

        private void RemoveEmptyRowsAndColumns()
        {
            RemoveEmptyRows();
            RemoveEmptyColumns();
        }

        public void InsertTile(TileConfiguration tile, TileInsertAction insertAction, TileConfiguration currentTile)
        {
            tile.ColumnNumber = currentTile.ColumnNumber;
            tile.RowNumber = currentTile.RowNumber;
            tile.ColumnSpan = 1;
            tile.RowSpan = 1;

            if (insertAction == TileInsertAction.ToRight)
            {
                InsertNewColumn(currentTile.ColumnNumber + currentTile.ColumnSpan);
                tile.ColumnNumber++;
            }
            else if (insertAction == TileInsertAction.ToLeft)
            {
                InsertNewColumn(currentTile.ColumnNumber);
            }
            else if (insertAction == TileInsertAction.Above)
            {
                InsertNewRow(currentTile.RowNumber);
            }
            else if (insertAction == TileInsertAction.Below)
            {
                InsertNewRow(currentTile.RowNumber + currentTile.ColumnSpan);
                tile.RowNumber++;
            }

            AddTile(tile);

            var tiles = new List<TileConfiguration>(_tile.Tiles) {tile};
            _tile.Tiles = tiles.ToArray();
        }

        public void AddTile(TileConfiguration tile)
        {
            if (tile.TypeId == TileConfiguration.BlankTileTypeId || tile.TypeId.Equals("6f1bf918-6080-42c2-b980-d562f77cb4e6",
                StringComparison.InvariantCultureIgnoreCase))
            {
                tile.TypeId = TileConfiguration.BlankTileTypeId;
                _childTiles.Add(tile, AddPanel(tile));
            }
            else
            {
                _childTiles.Add(tile, AddTile(tile, _tileFactory.Create(tile, this)));
            }
        }

        public void Clear()
        {
            _tileGrid.Children.Clear();
            _tileGrid.RowDefinitions.Clear();
            _tileGrid.ColumnDefinitions.Clear();
            _childTiles.Clear();
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

        private void RemoveEmptyRows()
        {
            var rowIndex = 0;
            while (rowIndex < _tileGrid.RowDefinitions.Count)
            {
                if (IsEmptyRow(rowIndex))
                {
                    DeletedRow(rowIndex);
                    continue;
                }
                rowIndex++;
            }
        }

        private void RemoveEmptyColumns()
        {
            var columnIndex = 0;
            while (columnIndex < _tileGrid.ColumnDefinitions.Count)
            {
                if (IsEmptyColumn(columnIndex))
                {
                    DeletedColumn(columnIndex);
                    continue;
                }
                columnIndex++;
            }
        }

        private void DeletedColumn(int toDeleteColumnIndex)
        {
            if (!IsEmptyColumn(toDeleteColumnIndex))
            {
                throw new InvalidOperationException();
            }

            var rowToDeleteNumber = toDeleteColumnIndex + 1;
            foreach (var tileConfiguration in _childTiles.Keys.Where(tileConfiguration => tileConfiguration.ColumnNumber >= rowToDeleteNumber))
            {
                tileConfiguration.ColumnNumber--;
            }

            foreach (UIElement child in _tileGrid.Children)
            {
                var rowIndex = Grid.GetColumn(child);
                if (rowIndex >= toDeleteColumnIndex)
                {
                    Grid.SetColumn(child, rowIndex - 1);
                }
            }

            _tileGrid.ColumnDefinitions.Remove(_tileGrid.ColumnDefinitions.Last());
        }

        private bool IsEmptyColumn(int rowIndex)
        {
            var rowNumber = rowIndex + 1;
            return !_childTiles.Keys.Any(x => x.ColumnNumber <= rowNumber && x.ColumnNumber + x.ColumnSpan - 1 >= rowNumber);
        }

        private void DeletedRow(int toDeleteRowIndex)
        {
            if (!IsEmptyRow(toDeleteRowIndex))
            {
                throw new InvalidOperationException();
            }

            var rowToDeleteNumber = toDeleteRowIndex + 1;
            foreach (var tileConfiguration in _childTiles.Keys.Where(tileConfiguration => tileConfiguration.RowNumber >= rowToDeleteNumber))
            {
                tileConfiguration.RowNumber--;
            }

            foreach (UIElement child in _tileGrid.Children)
            {
                var rowIndex = Grid.GetRow(child);
                if (rowIndex >= toDeleteRowIndex)
                {
                    Grid.SetRow(child, rowIndex - 1);
                }
            }

            _tileGrid.RowDefinitions.Remove(_tileGrid.RowDefinitions.Last());
        }

        private bool IsEmptyRow(int rowIndex)
        {
            var rowNumber = rowIndex + 1;
            return !_childTiles.Keys.Any(x => x.RowNumber <= rowNumber && x.RowNumber + x.RowSpan - 1 >= rowNumber);
        }

        private void InsertNewRow(int insertAtRowNumber)
        {
            var existingRowsCount = _tileGrid.RowDefinitions.Count;
            AddRowsToFit((insertAtRowNumber > existingRowsCount) ? insertAtRowNumber : existingRowsCount + 1, 1);

            foreach (var tileConfiguration in _childTiles.Keys.Where(tileConfiguration => tileConfiguration.RowNumber >= insertAtRowNumber))
            {
                tileConfiguration.RowNumber++;
            }

            foreach (UIElement child in _tileGrid.Children)
            {
                var rowIndex = Grid.GetRow(child);
                if (rowIndex >= insertAtRowNumber - 1)
                {
                    Grid.SetRow(child, rowIndex + 1);
                }
            }

            for (var columnNumber = 1; columnNumber < _tileGrid.ColumnDefinitions.Count; columnNumber++)
            {
                // add a placeholder tile
            }
        }

        private void InsertNewColumn(int insertAtColumnNumber)
        {
            var existingColumnsCount = _tileGrid.ColumnDefinitions.Count;
            AddColumnsToFit((insertAtColumnNumber > existingColumnsCount) ? insertAtColumnNumber : existingColumnsCount + 1, 1);

            foreach (var tileConfiguration in _childTiles.Keys.Where(tileConfiguration => tileConfiguration.ColumnNumber >= insertAtColumnNumber))
            {
                tileConfiguration.ColumnNumber++;
            }

            foreach (UIElement child in _tileGrid.Children)
            {
                var columnIndex = Grid.GetColumn(child);
                if (columnIndex >= insertAtColumnNumber - 1)
                {
                    Grid.SetColumn(child, columnIndex + 1);
                }
            }
        }

        private UIElement AddTile(TileConfiguration tile, IViewController viewController)
        {
            var panel = AddPlaceholderPanel(tile.RowNumber, tile.ColumnNumber, tile.RowSpan, tile.ColumnSpan);
            panel.Margin = new Thickness(2);
            var view = viewController.CreateView();
            panel.Children.Add(view);
            _dragAndDropController.RegisterTarget(view, this, tile);
            return panel;
        }

        private UIElement AddPanel(TileConfiguration tileConfiguration)
        {
            var panel = AddPlaceholderPanel(tileConfiguration.RowNumber, tileConfiguration.ColumnNumber, tileConfiguration.RowSpan, tileConfiguration.ColumnSpan);
            _tileLayoutControllerRegistry.GetNew(panel, tileConfiguration);
            return panel;
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

        public void Remove(TileConfiguration tile)
        {
            var element = _childTiles[tile];
            _childTiles.Remove(tile);
            _tileGrid.Children.Remove(element);
            _tile.Tiles = _tile.Tiles.Where(x => !ReferenceEquals(x, tile)).ToArray();
            RemoveEmptyRowsAndColumns();
        }
    }
}