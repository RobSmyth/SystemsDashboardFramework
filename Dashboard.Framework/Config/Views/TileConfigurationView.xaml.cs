using System.Windows;

namespace NoeticTools.Dashboard.Framework.Config.Views
{
    public partial class TileConfigurationView : Window
    {
        private readonly TileConfiguration _configuration;

        public TileConfigurationView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        public TileConfigurationView(TileConfiguration configuration)
        {
            _configuration = configuration;
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}