using System.Windows.Controls;


namespace NoeticTools.TeamStatusBoard.Tiles.PieChart
{
    public partial class PieChartTileControl : UserControl
    {
        public PieChartTileControl()
        {
            InitializeComponent();
        }

        internal void SetModel(PieChartTileViewModel model)
        {
            DataContext = model;
            model.SetView(chart);
        }
    }
}