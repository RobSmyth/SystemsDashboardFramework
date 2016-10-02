using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class TileRowSpanViewModel : NotifyingViewModelBase, INotifyingPropertyViewModel
    {
        private const int MaxSpan = 50;
        private readonly TileConfiguration _tile;
        private IDataValue _value;

        public TileRowSpanViewModel(TileConfiguration tile)
        {
            _tile = tile;
            Name = "Row span";
            EditorType = PropertyType.Text; // todo - numericspin
            Parameters = new object[0];
            _value = new DataValue("", _tile.RowSpan.ToString(), PropertiesFlags.None, OnValueChanged);
        }

        private void OnValueChanged()
        {
            var newValue = _value.Integer;
            if (IsValidChange(newValue))
            {
                _value.Integer = _tile.RowSpan;
                return;
            }
            _tile.RowSpan = _value.Integer;
            OnPropertyChanged("Value");
        }

        public IDataValue Value
        {
            get { return _value; }
            set
            {
                int newValue;
                if (int.TryParse(value.String, out newValue))
                {
                    if (IsValidChange(newValue))
                    {
                        return;
                    }
                    _value = value;
                    _tile.RowSpan = newValue;
                    OnPropertyChanged();
                }
            }
        }

        public string Name { get; }
        public PropertyType EditorType { get; }
        public object[] Parameters { get; }

        private bool IsValidChange(int newValue)
        {
            return _tile.RowSpan == newValue || newValue > MaxSpan || newValue <= 0;
        }
    }
}