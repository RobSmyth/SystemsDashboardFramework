using System;
using System.Linq;
using System.Threading.Tasks;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class PropertyViewModel : NotifyingViewModelBase, INotifyingPropertyViewModel
    {
        private readonly INamedValueRepository _tileConfiguration;
        private readonly ISuggestionProvider<object> _suggestionProvider;
        private object[] _parameters;
        private IDataValue _value;

        public PropertyViewModel(string name, PropertyType editorType, INamedValueRepository tileConfiguration, ISuggestionProvider<object> suggestionProvider)
        {
            _tileConfiguration = tileConfiguration;
            _suggestionProvider = suggestionProvider;
            Name = name;
            EditorType = editorType;
            _value = Subscribe(new DataValue(name, "", PropertiesFlags.None, () => { }), name, _tileConfiguration.GetString(Name));
        }

        public IDataValue Value
        {
            get { return _value; }
            set
            {
                if (Equals(value, _value)) return;
                _value = value;
                _tileConfiguration.SetParameter(Name, _value.String);
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

        private void UpdateParameters()
        {
            Parameters = _suggestionProvider.Get().ToArray();
        }

        private IDataValue Subscribe(IDataValue existing, string propertyName, string defaultValue)
        {
            var dataValue = string.IsNullOrWhiteSpace(propertyName) ? (IDataValue)new NullDataValue() 
                : new DataValue("Value", _tileConfiguration.GetString(propertyName, defaultValue), PropertiesFlags.ReadWrite, () => { });

            if (ReferenceEquals(existing, dataValue))
            {
                return existing;
            }

            if (dataValue.NotSet)
            {
                dataValue.Instance = defaultValue;
            }

            existing.Broadcaster.RemoveListener(this);
            dataValue.Broadcaster.AddListener(this, () =>
            {
                _tileConfiguration.SetParameter(Name, dataValue.String);
                OnPropertyChanged(propertyName);
            });

            return dataValue;
        }
    }
}