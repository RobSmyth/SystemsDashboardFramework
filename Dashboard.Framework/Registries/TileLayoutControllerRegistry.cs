using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public class TileLayoutControllerRegistry : ITileLayoutControllerRegistry
    {
        private readonly ITileControllerFactory _tileFactory;
        private readonly TileDragAndDropController _dragAndDropController;
        private readonly IList<ITileLayoutController> _layoutControllers = new List<ITileLayoutController>();

        public TileLayoutControllerRegistry(ITileControllerFactory tileFactory, TileDragAndDropController dragAndDropController)
        {
            _tileFactory = tileFactory;
            _dragAndDropController = dragAndDropController;
        }

        public ITileLayoutController GetNew(Grid tileGrid, TileConfiguration tileConfiguration)
        {
            var layoutController = new TileLayoutController(tileGrid, _tileFactory, this, new Thickness(0), _dragAndDropController);
            _layoutControllers.Add(layoutController);
            layoutController.Load(tileConfiguration);
            return layoutController;
        }

        public ITileLayoutController[] GetAll()
        {
            return _layoutControllers.ToArray();
        }

        public int Count => _layoutControllers.Count;
    }
}