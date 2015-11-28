using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace NoeticTools.Dashboard.Framework.Tiles.InsertTile
{
    public partial class InsertTileControl : UserControl
    {
        private Point _mouseDownPoint;

        public InsertTileControl()
        {
            InitializeComponent();
        }

        private void ProvidersList_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDownPoint = e.GetPosition(null);
        }

        private void ProvidersList_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            var mousePos = e.GetPosition(null);
            var diff = _mouseDownPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed && 
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                if (providersList.SelectedItem == null)
                {
                    return;
                }

                var dragData = new DataObject("TileProviderName", ((ITileControllerProvider)providersList.SelectedItem).Name);
                DragDrop.DoDragDrop(providersList, dragData, DragDropEffects.Copy);
            }
        }
    }
}
