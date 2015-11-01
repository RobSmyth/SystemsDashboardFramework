using System;
using System.Windows;
using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public class TileLayoutController : ITileLayoutController
    {
        private readonly Thickness _highlightedMargin = new Thickness(10);
        private readonly Thickness _normalMargin;
        private readonly Grid _tileGrid;
        private readonly ITileLayoutControllerRegistry _tileLayoutControllerRegistry;
        private readonly ITileRegistry _tileRegistry;

        public TileLayoutController(Grid tileGrid, ITileRegistry tileRegistry,
            ITileLayoutControllerRegistry tileLayoutControllerRegistry, Thickness normalMargin)
        {
            _tileRegistry = tileRegistry;
            _tileLayoutControllerRegistry = tileLayoutControllerRegistry;
            _normalMargin = normalMargin;
            _tileGrid = tileGrid;
            _tileGrid.Margin = _normalMargin;
        }

        public void AddTile(DashboardTileConfiguration tileConfiguration)
        {
            ITileLayoutController layoutController = null;

            if (tileConfiguration.TypeId == DashboardTileConfiguration.BlankTileTypeId ||
                tileConfiguration.TypeId.Equals("6f1bf918-6080-42c2-b980-d562f77cb4e6",
                    StringComparison.InvariantCultureIgnoreCase))
            {
                tileConfiguration.TypeId = DashboardTileConfiguration.BlankTileTypeId;
                layoutController = AddPanel(tileConfiguration);
            }
            else
            {
                layoutController = AddTile(tileConfiguration, _tileRegistry.GetNew(tileConfiguration));
            }

            foreach (var tile in tileConfiguration.Tiles)
            {
                layoutController.AddTile(tile);
            }
        }

        public void Clear()
        {
            _tileGrid.Children.Clear();
            _tileGrid.RowDefinitions.Clear();
            _tileGrid.ColumnDefinitions.Clear();
            _tileRegistry.Clear();
        }

        public void ToggleDisplayMode()
        {
            if (_tileGrid.Margin == _normalMargin)
            {
                _tileGrid.Margin = _highlightedMargin;
                // todo - adorner ... perhaps show model of panels with # and list of settings?
            }
            else
            {
                _tileGrid.Margin = _normalMargin;
            }
        }

        private TileLayoutController AddTile(DashboardTileConfiguration tileConfiguration, ITileViewModel viewModel)
        {
            var panel = AddPlaceholderPanel(tileConfiguration.RowNumber, tileConfiguration.ColumnNumber,
                tileConfiguration.RowSpan, tileConfiguration.ColumnSpan);
            panel.Margin = new Thickness(2);
            var view = viewModel.CreateView();
            panel.Children.Add(view);
            return this;
        }

        private ITileLayoutController AddPanel(DashboardTileConfiguration tileConfiguration)
        {
            var panel = AddPlaceholderPanel(tileConfiguration.RowNumber, tileConfiguration.ColumnNumber,
                tileConfiguration.RowSpan, tileConfiguration.ColumnSpan);
            var layout = _tileLayoutControllerRegistry.GetNew(panel);
            return layout;
        }

        private Grid AddPlaceholderPanel(int rowNumber, int columnNumber, int rowSpan, int columnSpan)
        {
            while (rowNumber + rowSpan - 1 > _tileGrid.RowDefinitions.Count)
            {
                _tileGrid.RowDefinitions.Add(new RowDefinition());
            }

            while (columnNumber + columnSpan - 1 > _tileGrid.ColumnDefinitions.Count)
            {
                _tileGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            var panel = new Grid {Margin = new Thickness(0)};

            Grid.SetRow(panel, rowNumber - 1);
            Grid.SetColumn(panel, columnNumber - 1);
            Grid.SetRowSpan(panel, rowSpan);
            Grid.SetColumnSpan(panel, columnSpan);

            _tileGrid.Children.Add(panel);
            return panel;
        }
    }
}