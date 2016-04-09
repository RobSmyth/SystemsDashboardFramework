using System.Windows;
using System.Windows.Controls;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.InsertTile
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
            var dragData = new DataObject(typeof (TileConfiguration), tileConfiguration);
            DragDrop.DoDragDrop(providersList, dragData, DragDropEffects.Copy);
        }
    }
}