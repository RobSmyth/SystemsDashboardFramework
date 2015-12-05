using System.Linq;


namespace NoeticTools.Dashboard.Framework.Config.Properties
{
    public class PropertyViewModel : NotifyingViewModelBase, INotifyingElementViewModel
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private object[] _parameters;

        public PropertyViewModel(string name, string viewerName, TileConfigurationConverter tileConfigurationConverter, params string[] parameters)
        {
            _tileConfigurationConverter = tileConfigurationConverter;
            Name = name;
            ViewerName = viewerName;
            Parameters = parameters.Cast<object>().ToArray();
        }

        public object Value
        {
            get { return _tileConfigurationConverter.GetParameter(Name); }
            set
            {
                var currentValue = _tileConfigurationConverter.GetParameter(Name);
                if (Equals(value, currentValue)) return;
                _tileConfigurationConverter.SetParameter(Name, value);
                OnPropertyChanged();
            }
        }

        public string Name { get; }
        public string ViewerName { get; }

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