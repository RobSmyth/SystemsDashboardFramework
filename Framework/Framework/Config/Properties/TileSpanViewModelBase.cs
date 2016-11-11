using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public abstract class TileSpanViewModelBase : NotifyingViewModelBase, INotifyingPropertyViewModel
    {
        private const int MaxSpan = 50;
        protected readonly TileConfiguration TileConfiguration;
        private IDataValue _value;

        protected TileSpanViewModelBase(TileConfiguration tileConfiguration, string name)
        {
            Name = name;
            TileConfiguration = tileConfiguration;
            EditorType = PropertyType.Text; // todo - numericspin
            Parameters = new object[0];
            _value = new DataValue("", Span.ToString(), PropertiesFlags.None);
        }

        protected abstract int Span { get; set; }

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
                    Span = newValue;
                    OnPropertyChanged();
                }
            }
        }

        public string Name { get; }
        public PropertyType EditorType { get; }
        public object[] Parameters { get; }

        private bool IsValidChange(int newValue)
        {
            return Span == newValue || newValue > MaxSpan || newValue <= 0;
        }

        private void OnValueChanged()
        {
            var newValue = _value.Integer;
            if (IsValidChange(newValue))
            {
                _value.Integer = TileConfiguration.RowSpan;
                return;
            }
            Span = _value.Integer;
            OnPropertyChanged("Value");
        }
    }
}