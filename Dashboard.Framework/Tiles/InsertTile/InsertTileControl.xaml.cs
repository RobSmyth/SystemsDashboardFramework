using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles.InsertTile
{
    public partial class InsertTileControl : UserControl, IDragSource
    {
        public InsertTileControl()
        {
            InitializeComponent();
        }

        public UIElement Element => this;

        public void OnMouseDragStarted()
        {
            if (providersList.SelectedItem == null || !providersList.IsMouseOver)
            {
                return;
            }

            var tileConfiguration = ((ITileControllerProvider) providersList.SelectedItem).CreateDefaultConfiguration();
            var dragData = new DataObject(typeof(TileConfiguration), tileConfiguration);
            DragDrop.DoDragDrop(providersList, dragData, DragDropEffects.Copy);
        }
    }
}
