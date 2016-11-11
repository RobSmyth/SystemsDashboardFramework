using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public sealed class TileLayoutControllerRegistry : ITileLayoutControllerRegistry
    {
        private readonly ITileFactory _tileFactory;
        private readonly TileDragAndDropController _dragAndDropController;
        private readonly DashboardTileNavigator _tileNavigator;
        private readonly TsbCommands _commandsBindings;
        private readonly IList<ITileLayoutController> _layoutControllers = new List<ITileLayoutController>();

        public TileLayoutControllerRegistry(ITileFactory tileFactory, TileDragAndDropController dragAndDropController, DashboardTileNavigator tileNavigator, TsbCommands commandsBindings)
        {
            _tileFactory = tileFactory;
            _dragAndDropController = dragAndDropController;
            _tileNavigator = tileNavigator;
            _commandsBindings = commandsBindings;
        }

        public int Count => _layoutControllers.Count;

        public void GetNew(Grid tileGrid, TileConfiguration tileConfiguration, TileLayoutController parent)
        {
            var layoutController = new TileLayoutController(tileGrid, _tileFactory, this, new Thickness(0), _dragAndDropController, _tileNavigator, _commandsBindings);
            _layoutControllers.Add(layoutController);
            layoutController.Load(tileConfiguration);
        }

        public IEnumerable<ITileLayoutController> GetAll()
        {
            return _layoutControllers.ToArray();
        }
    }
}