using System;
using System.Linq;
using System.Threading.Tasks;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class PropertyViewModel : NotifyingViewModelBase, INotifyingPropertyViewModel
    {
        private readonly INamedValueRepository _tileConfiguration;
        private readonly ISuggestionProvider<object> _suggestionProvider;
        private object[] _parameters;

        public PropertyViewModel(string name, PropertyType editorType, INamedValueRepository tileConfiguration, ISuggestionProvider<object> suggestionProvider)
        {
            _tileConfiguration = tileConfiguration;
            _suggestionProvider = suggestionProvider;
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
            Parameters = _suggestionProvider.Get().ToArray();
            //Task.Factory.StartNew(() => _suggestionProvider.Get().Cast<object>().ToArray()).ContinueWith(x => Parameters = x.Result);
        }
    }
}