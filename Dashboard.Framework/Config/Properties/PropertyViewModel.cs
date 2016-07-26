using System;
using System.Threading.Tasks;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class PropertyViewModel : NotifyingViewModelBase, INotifyingElementViewModel
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly Func<object[]> _parametersFunc;
        private object[] _parameters;

        public PropertyViewModel(string name, string viewerName, TileConfigurationConverter tileConfigurationConverter, Func<object[]> parametersFunc = null)
        {
            _tileConfigurationConverter = tileConfigurationConverter;
            _parametersFunc = parametersFunc;
            Name = name;
            ViewerName = viewerName;
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
            get
            {
                if (_parameters == null)
                {
                    UpdateParameters();
                }
                return _parameters ?? new object[0];
            }
            set
            {
                if (_parameters != value)
                {
                    _parameters = value;
                    OnPropertyChanged();
                }
            }
        }

        protected void UpdateParameters()
        {
            Task.Factory.StartNew(() => _parametersFunc()).ContinueWith(x => Parameters = x.Result);
        }
    }
}