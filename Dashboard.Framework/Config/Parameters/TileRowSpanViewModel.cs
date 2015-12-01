namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class TileRowSpanViewModel : NotifyingViewModelBase, INotifyingElementViewModel
    {
        private readonly TileConfiguration _tile;
        private const int MaxSpan = 50;

        public TileRowSpanViewModel(TileConfiguration tile)
        {
            _tile = tile;
            Name = "Row span";
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
                    if (_tile.RowSpan == newValue || newValue > MaxSpan || newValue <= 0)
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