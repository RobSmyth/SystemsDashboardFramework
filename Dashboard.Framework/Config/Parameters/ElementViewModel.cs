

namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class ElementViewModel : NotifyingViewModelBase, IElementViewModel
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;

        public ElementViewModel(string name, ElementType elementType, TileConfigurationConverter tileConfigurationConverter)
        {
            _tileConfigurationConverter = tileConfigurationConverter;
            Name = name;
            ElementType = elementType;
            Parameters = new object[0];
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
        public object[] Parameters { get; }
    }
}