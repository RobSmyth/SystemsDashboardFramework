using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Registries;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public sealed class TileLayoutControllerRegistry : ITileLayoutControllerRegistry
    {
        private readonly ITileFactory _tileFactory;
        private readonly TileDragAndDropController _dragAndDropController;
        private readonly DashboardTileNavigator _tileNavigator;
        private readonly ApplicationCommandsBindings _commandsBindings;
        private readonly IList<ITileLayoutController> _layoutControllers = new List<ITileLayoutController>();

        public TileLayoutControllerRegistry(ITileFactory tileFactory, TileDragAndDropController dragAndDropController, DashboardTileNavigator tileNavigator, ApplicationCommandsBindings commandsBindings)
        {
            _tileFactory = tileFactory;
            _dragAndDropController = dragAndDropController;
            _tileNavigator = tileNavigator;
            _commandsBindings = commandsBindings;
        }

        public int Count => _layoutControllers.Count;

        public ITileLayoutController GetNew(Grid tileGrid, TileConfiguration tileConfiguration, TileLayoutController parent)
        {
            var layoutController = new TileLayoutController(tileGrid, _tileFactory, this, new Thickness(0), _dragAndDropController, _tileNavigator, parent, _commandsBindings);
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