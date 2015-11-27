

namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class ElementViewModel : NotifyingViewModelBase, INotifyingElementViewModel
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private object[] _parameters;

        public ElementViewModel(string name, ElementType elementType, TileConfigurationConverter tileConfigurationConverter, params  string[] parameters)
        {
            _tileConfigurationConverter = tileConfigurationConverter;
            Name = name;
            ElementType = elementType;
            Parameters = parameters;
        }

        public object Value
        {
            get { return _tileConfigurationConverter.GetParameter(Name, ElementType); }
            set
            {
                var currentValue = _tileConfigurationConverter.GetParameter(Name, ElementType);
                if (Equals(value, currentValue)) return;
                _tileConfigurationConverter.SetParameter(Name, ElementType, value);
                OnPropertyChanged();
            }
        }

        public string Name { get; }
        public ElementType ElementType { get; }

        public object[] Parameters
        {
            get { return _parameters; }
            set
            {
                if (Equals(value, _parameters)) return;
                _parameters = value;
                OnPropertyChanged();
            }
        }
    }
}