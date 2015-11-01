using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public class TileLayoutControllerRegistry : ITileLayoutControllerRegistry
    {
        private readonly ITileRegistry _tileRegistry;
        private readonly IList<ITileLayoutController> _layoutControllers = new List<ITileLayoutController>();

        public TileLayoutControllerRegistry(ITileRegistry tileRegistry)
        {
            _tileRegistry = tileRegistry;
        }

        public ITileLayoutController GetNew(Grid tileGrid)
        {
            var layoutController = new TileLayoutController(tileGrid, _tileRegistry, this, new Thickness(0));
            _layoutControllers.Add(layoutController);
            return layoutController;
        }

        public ITileLayoutController[] GetAll()
        {
            return _layoutControllers.ToArray();
        }
    }
}