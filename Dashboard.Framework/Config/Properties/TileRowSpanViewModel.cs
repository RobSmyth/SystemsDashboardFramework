namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class TileRowSpanViewModel : NotifyingViewModelBase, INotifyingPropertyViewModel
    {
        private const int MaxSpan = 50;
        private readonly TileConfiguration _tile;

        public TileRowSpanViewModel(TileConfiguration tile)
        {
            _tile = tile;
            Name = "Row span";
            EditorType = PropertyType.Text; // todo - numericspin
            Parameters = new object[0];
        }

        public object Value
        {
            get { return _tile.RowSpan.ToString(); }
            set
            {
                int newValue;
                if (int.TryParse((string) value, out newValue))
                {
                    if (_tile.RowSpan == newValue || newValue > MaxSpan || newValue <= 0)
                    {
                        return;
                    }
                    _tile.RowSpan = newValue;
                    OnPropertyChanged();
                }
            }
        }

        public void UpdateParameters()
        {
            throw new System.NotImplementedException();
        }

        public string Name { get; }
        public PropertyType EditorType { get; }
        public object[] Parameters { get; set; }
    }
}