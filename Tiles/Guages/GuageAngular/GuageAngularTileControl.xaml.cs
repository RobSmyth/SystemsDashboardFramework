using System.Windows.Controls;


namespace NoeticTools.TeamStatusBoard.Tiles.Guages.GuageAngular
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
            model.InitialiseGuageView(guage);
        }
    }
}