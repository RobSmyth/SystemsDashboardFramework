using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Input;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public sealed class TileLayoutControllerRegistry : ITileLayoutControllerRegistry
    {
        private readonly ITileControllerFactory _tileFactory;
        private readonly TileDragAndDropController _dragAndDropController;
        private readonly DashboardTileNavigator _tileNavigator;
        private readonly IList<ITileLayoutController> _layoutControllers = new List<ITileLayoutController>();

        public TileLayoutControllerRegistry(ITileControllerFactory tileFactory, TileDragAndDropController dragAndDropController, DashboardTileNavigator tileNavigator)
        {
            _tileFactory = tileFactory;
            _dragAndDropController = dragAndDropController;
            _tileNavigator = tileNavigator;
        }

        public int Count => _layoutControllers.Count;

        public ITileLayoutController GetNew(Grid tileGrid, TileConfiguration tileConfiguration, TileLayoutController parent)
        {
            var layoutController = new TileLayoutController(tileGrid, _tileFactory, this, new Thickness(0), _dragAndDropController, _tileNavigator, parent);
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