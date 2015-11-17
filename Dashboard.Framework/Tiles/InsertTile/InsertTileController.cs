using System.Windows;


namespace NoeticTools.Dashboard.Framework.Tiles.InsertTile
{
    public class InsertTileController : IViewController
    {
        public FrameworkElement CreateView()
        {
            return new InsertTileControl() { DataContext = this };
        }

        public void OnConfigurationChanged()
        {
        }
    }
}