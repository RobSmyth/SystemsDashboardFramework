using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Adorners;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.BlankTile;
using NoeticTools.SystemsDashboard.Framework.Registries;


namespace NoeticTools.SystemsDashboard.Framework
{
    public sealed class TileLayoutController : ITileLayoutController
    {
        private readonly Thickness _normalMargin;
        private readonly Thickness _tileMargin = new Thickness(2);
        private readonly TileDragAndDropController _dragAndDropController;
        private readonly DashboardTileNavigator _tileNavigator;
        private readonly Grid _tileGrid;
        private readonly ITileLayoutControllerRegistry _layoutControllerRegistry;
        private readonly ITileFactory _tileFactory;
        private readonly IDictionary<TileConfiguration, UIElement> _tileToView = new Dictionary<TileConfiguration, UIElement>();
        private readonly TileLayoutController _parent;
        private readonly RoutedCommands _commands;
        private TileConfiguration _tile;

        public TileLayoutController(Grid tileGrid, ITileFactory tileFactory, ITileLayoutControllerRegistry layoutControllerRegistry, Thickness normalMargin, 
            TileDragAndDropController dragAndDropController, DashboardTileNavigator tileNavigator, TileLayoutController parent, RoutedCommands commands)
        {
            _tileFactory = tileFactory;
            _layoutControllerRegistry = layoutControllerRegistry;
            _normalMargin = normalMargin;
            _dragAndDropController = dragAndDropController;
            _tileNavigator = tileNavigator;
            _parent = parent;
            _commands = commands;
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

        public void InsertTile(TileConfiguration currentTile, TileConfiguration tile, TileInsertAction insertAction)
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

            RemoveAt(tile.RowNumber, tile.ColumnNumber);
            AddTile(tile);
            AddToConfiguration(tile);
        }

        public void Clear()
        {
            _tileGrid.Children.Clear();
            _tileGrid.RowDefinitions.Clear();
            _tileGrid.ColumnDefinitions.Clear();
            _tileToView.Clear();
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

        public void Remove(TileConfiguration tile)
        {
            var row = tile.RowNumber;
            var rowSpan = tile.RowSpan;

            RemoveTile(tile);

            for (var rowNumber = row; rowNumber < row + rowSpan; rowNumber++)
            {
                FillRow(rowNumber);
            }

            RemoveEmptyRowsAndColumns();
        }

        public bool ProvdesLayoutFor(UIElement element)
        {
            return ReferenceEquals(_tileGrid, element);
        }

        public void SplitTile(TileConfiguration tile, TileConfiguration newTile, TileInsertAction insertAction)
        {
            if (_tile.Tiles.Length == 1)
            {
                InsertTile(tile, newTile, insertAction);
                return;
            }

            var panel = new TileConfiguration()
            {
                TypeId = TileConfiguration.PaneTileTypeId,
                Tiles = new[] {tile}
            };
            panel.SetLocation(tile);
            RemoveTile(tile);

            tile.RowNumber = 1;
            tile.ColumnNumber = 1;
            AddTile(panel);
            AddToConfiguration(panel);

            var layoutController = _layoutControllerRegistry.GetAll().Single(x => x.ProvdesLayoutFor(_tileToView[panel]));
            layoutController.InsertTile(tile, newTile, insertAction);
        }

        public void Replace(TileConfiguration tileToReplace, TileConfiguration newTile)
        {
            newTile.SetLocation(tileToReplace);
            Remove(tileToReplace);
            AddTile(newTile);
            AddToConfiguration(newTile);
        }

        private void AddToConfiguration(TileConfiguration tile)
        {
            _tile.Tiles = new List<TileConfiguration>(_tile.Tiles) {tile}.ToArray();
        }

        public void AddTile(TileConfiguration tile)
        {
            if (tile.TypeId == TileConfiguration.PaneTileTypeId || tile.TypeId.Equals("6f1bf918-6080-42c2-b980-d562f77cb4e6", StringComparison.InvariantCultureIgnoreCase))
            {
                tile.TypeId = TileConfiguration.PaneTileTypeId;
                _tileToView.Add(tile, AddPanel(tile));
            }
            else
            {
                _tileToView.Add(tile, AddTile(tile, _tileFactory.Create(tile, this)));
            }
        }

        private void RemoveEmptyRowsAndColumns()
        {
            RemoveEmptyRows();
            RemoveEmptyColumns();
        }

        private void RemoveAt(int rowNumber, int columnNumber)
        {
            foreach (var tile in _tileToView.Keys.Where(x => x.IsInRow(rowNumber) && x.IsInColumn(columnNumber)).ToArray())
            {
                RemoveTile(tile);
            }
        }

        private void RemoveEmptyRows()
        {
            var rowIndex = 0;
            while (rowIndex < _tileGrid.RowDefinitions.Count)
            {
                if (IsEmptyRow(rowIndex))
                {
                    RemoveRow(rowIndex);
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
                    RemoveColumn(columnIndex);
                    continue;
                }
                columnIndex++;
            }
        }

        private void RemoveColumn(int toDeleteColumnIndex)
        {
            for (var rowNumber = 1; rowNumber <= _tileGrid.ColumnDefinitions.Count; rowNumber++)
            {
                RemoveAt(rowNumber, toDeleteColumnIndex + 1);
            }

            if (!IsEmptyColumn(toDeleteColumnIndex))
            {
                throw new InvalidOperationException();
            }

            var rowToDeleteNumber = toDeleteColumnIndex + 1;
            foreach (var tileConfiguration in _tileToView.Keys.Where(tileConfiguration => tileConfiguration.ColumnNumber >= rowToDeleteNumber))
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

        private bool IsEmptyColumn(int columnIndex)
        {
            var columnNumber = columnIndex + 1;
            return !_tileToView.Keys.Any(x => x.IsInColumn(columnNumber) && x.TypeId != BlankTilePlugin.TileTypeId);
        }

        private void RemoveRow(int toDeleteRowIndex)
        {
            for (var columnNumber = 1; columnNumber <= _tileGrid.ColumnDefinitions.Count; columnNumber++)
            {
                RemoveAt(toDeleteRowIndex + 1, columnNumber);
            }

            if (!IsEmptyRow(toDeleteRowIndex))
            {
                throw new InvalidOperationException();
            }

            var rowToDeleteNumber = toDeleteRowIndex + 1;
            foreach (var tileConfiguration in _tileToView.Keys.Where(tileConfiguration => tileConfiguration.RowNumber >= rowToDeleteNumber))
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
            return !_tileToView.Keys.Any(x => x.IsInRow(rowNumber) && x.TypeId != BlankTilePlugin.TileTypeId);
        }

        private void InsertNewRow(int insertAtRowNumber)
        {
            var existingRowsCount = _tileGrid.RowDefinitions.Count;
            AddRowsToFit((insertAtRowNumber > existingRowsCount) ? insertAtRowNumber : existingRowsCount + 1, 1);

            foreach (var tileConfiguration in _tileToView.Keys.Where(tileConfiguration => tileConfiguration.RowNumber >= insertAtRowNumber))
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

            FillRow(insertAtRowNumber);
        }

        private void FillRow(int rowNumber)
        {
            for (var columnNumber = 1; columnNumber <= _tileGrid.ColumnDefinitions.Count; columnNumber++)
            {
                if (!HasTile(rowNumber, columnNumber))
                {
                    AddBlankTile(rowNumber, columnNumber);
                }
            }
        }

        private bool HasTile(int rowNumber, int columnNumber)
        {
            return _tileToView.Keys.Any(x => x.IsInRow(rowNumber) && x.IsInColumn(columnNumber));
        }

        private void AddBlankTile(int rowNumber, int columnNumber)
        {
            var blankTile = new TileConfiguration
            {
                TypeId = BlankTilePlugin.TileTypeId,
                RowNumber = rowNumber,
                ColumnNumber = columnNumber,
                RowSpan = 1,
                ColumnSpan = 1,
                Tiles = new TileConfiguration[0]
            };
            _tileToView.Add(blankTile, AddTile(blankTile, _tileFactory.Create(blankTile, this)));
            AddToConfiguration(blankTile);
        }

        private void InsertNewColumn(int insertAtColumnNumber)
        {
            var existingColumnsCount = _tileGrid.ColumnDefinitions.Count;
            AddColumnsToFit((insertAtColumnNumber > existingColumnsCount) ? insertAtColumnNumber : existingColumnsCount + 1, 1);

            foreach (var tileConfiguration in _tileToView.Keys.Where(tileConfiguration => tileConfiguration.ColumnNumber >= insertAtColumnNumber))
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

            FillColumn(insertAtColumnNumber);
        }

        private void FillColumn(int insertAtColumnNumber)
        {
            for (var rowNumber = 1; rowNumber <= _tileGrid.RowDefinitions.Count; rowNumber++)
            {
                AddBlankTile(rowNumber, insertAtColumnNumber);
            }
        }

        private UIElement AddTile(TileConfiguration tile, FrameworkElement view)
        {
            var panel = AddPlaceholderPanel(tile.RowNumber, tile.ColumnNumber, tile.RowSpan, tile.ColumnSpan);
            panel.Margin = _tileMargin;
            panel.MouseLeftButtonDown += Panel_MouseLeftButtonDown;

            panel.Children.Add(view);

            _commands.BindView(tile, view, this);

            _dragAndDropController.RegisterTarget(view, this, tile);
            _dragAndDropController.Register(view);

            return panel;
        }

        private void Panel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _tileNavigator.MoveTo(sender as UIElement);
        }

        private UIElement AddPanel(TileConfiguration tile)
        {
            var panel = AddPlaceholderPanel(tile.RowNumber, tile.ColumnNumber, tile.RowSpan, tile.ColumnSpan);
            _layoutControllerRegistry.GetNew(panel, tile, this);
            return panel;
        }

        private Grid AddPlaceholderPanel(int rowNumber, int columnNumber, int rowSpan, int columnSpan)
        {
            AddRowsToFit(rowNumber, rowSpan);
            AddColumnsToFit(columnNumber, columnSpan);

            var groupPanel = new Grid {Name = $"Panel{_layoutControllerRegistry.Count + 1}"};

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

        private void RemoveTile(TileConfiguration tile)
        {
            var element = _tileToView[tile];
            _dragAndDropController.DeRegister(element);
            _tileToView.Remove(tile);
            _tileGrid.Children.Remove(element);
            _tile.Tiles = _tile.Tiles.Where(x => !ReferenceEquals(x, tile)).ToArray();
        }
    }
}