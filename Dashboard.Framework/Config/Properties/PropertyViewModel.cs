using System;
using System.Linq;


namespace NoeticTools.SystemsDashboard.Framework.Config.Properties
{
    public class PropertyViewModel : NotifyingViewModelBase, INotifyingElementViewModel
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly Func<object[]> _parametersFunc;
        //private object[] _parameters;

        public PropertyViewModel(string name, string viewerName, TileConfigurationConverter tileConfigurationConverter, Func<object[]> parametersFunc = null)
        {
            _tileConfigurationConverter = tileConfigurationConverter;
            _parametersFunc = parametersFunc;
            Name = name;
            ViewerName = viewerName;
            //Parameters = parameters.Cast<object>().ToArray();
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

        public object[] Parameters => _parametersFunc != null ? _parametersFunc() : new object[0];
    }
}