using System;
using System.Threading.Tasks;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class PropertyViewModel : NotifyingViewModelBase, INotifyingPropertyViewModel
    {
        private readonly INamedValueRepository _tileConfiguration;
        private readonly Func<object[]> _parametersFunc;
        private object[] _parameters;

        public PropertyViewModel(string name, PropertyType editorType, INamedValueRepository tileConfiguration, Func<object[]> parametersFunc = null)
        {
            _tileConfiguration = tileConfiguration;
            _parametersFunc = parametersFunc;
            Name = name;
            EditorType = editorType;
        }

        public object Value
        {
            get { return _tileConfiguration.GetParameter(Name); }
            set
            {
                var currentValue = _tileConfiguration.GetParameter(Name);
                if (Equals(value, currentValue)) return;
                _tileConfiguration.SetParameter(Name, value);
                OnPropertyChanged();
            }
        }

        public string Name { get; }
        public PropertyType EditorType { get; }

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