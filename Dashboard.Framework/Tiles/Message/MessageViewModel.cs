using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Parameters;


namespace NoeticTools.Dashboard.Framework.Tiles.Message
{
    internal class MessageViewModel : NotifyingViewModelBase, IConfigurationChangeListener
    {
        public static readonly string TileTypeId = "Message";
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private string _text;

        public MessageViewModel(TileConfiguration tileConfiguration, IDashboardController dashboardController)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tileConfiguration, this);

            ConfigureCommand = new TileConfigureCommand("Message Tile Configuration", new[]
            {
                new ElementViewModel("Message", ElementType.Text, _tileConfigurationConverter)
            },
                dashboardController);
            Update();
        }

        public string Text
        {
            get { return _text; }
            private set
            {
                if (value == _text) return;
                _text = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfigureCommand { get; private set; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            Update();
        }

        private void Update()
        {
            Text = _tileConfigurationConverter.GetString("Message");
        }
    }
}