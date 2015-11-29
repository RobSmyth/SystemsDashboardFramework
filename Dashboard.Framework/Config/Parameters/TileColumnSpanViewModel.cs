namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class TileColumnSpanViewModel : NotifyingViewModelBase, INotifyingElementViewModel
    {
        private readonly TileConfiguration _tile;
        private const int MaxRowSPan = 50;

        public TileColumnSpanViewModel(TileConfiguration tile)
        {
            _tile = tile;
            Name = "Column span";
            ElementType = ElementType.Text;// todo - numericspin
            Parameters = new object[0];
        }

        public object Value
        {
            get { return _tile.RowSpan.ToString(); }
            set
            {
                int newValue;
                if (int.TryParse((string)value, out newValue))
                {
                    if (_tile.RowSpan == newValue || newValue > MaxRowSPan || newValue <= 0)
                    {
                        return;
                    }
                    _tile.RowSpan = newValue;
                    OnPropertyChanged();
                }
            }
        }

        public string Name { get; }
        public ElementType ElementType { get; }
        public object[] Parameters { get; set; }
    }
}