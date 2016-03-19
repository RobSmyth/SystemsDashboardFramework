using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Input;


namespace NoeticTools.SystemsDashboard.Framework.Registries
{
    public sealed class TileLayoutControllerRegistry : ITileLayoutControllerRegistry
    {
        private readonly ITileFactory _tileFactory;
        private readonly TileDragAndDropController _dragAndDropController;
        private readonly DashboardTileNavigator _tileNavigator;
        private readonly RoutedCommands _commands;
        private readonly IList<ITileLayoutController> _layoutControllers = new List<ITileLayoutController>();

        public TileLayoutControllerRegistry(ITileFactory tileFactory, TileDragAndDropController dragAndDropController, DashboardTileNavigator tileNavigator, RoutedCommands commands)
        {
            _tileFactory = tileFactory;
            _dragAndDropController = dragAndDropController;
            _tileNavigator = tileNavigator;
            _commands = commands;
        }

        public int Count => _layoutControllers.Count;

        public ITileLayoutController GetNew(Grid tileGrid, TileConfiguration tileConfiguration, TileLayoutController parent)
        {
            var layoutController = new TileLayoutController(tileGrid, _tileFactory, this, new Thickness(0), _dragAndDropController, _tileNavigator, parent, _commands);
            _layoutControllers.Add(layoutController);
            layoutController.Load(tileConfiguration);
            return layoutController;
        }

        public ITileLayoutController[] GetAll()
        {
            return _layoutControllers.ToArray();
        }
    }
}