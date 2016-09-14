using System.Windows.Controls;
using System.Windows.Media.Effects;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.Guages.GuageAngular
{
    public partial class GuageAngularTileControl : UserControl
    {
        private GuageAngularTileViewModel _model;

        public GuageAngularTileControl()
        {
            InitializeComponent();
        }

        internal void SetModel(GuageAngularTileViewModel model)
        {
            _model = model;
            DataContext = model;
            model.SetView(this.guage);
        }
    }
}