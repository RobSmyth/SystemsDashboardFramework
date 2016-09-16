using System;
using System.ComponentModel;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class DependantPropertyViewModel : PropertyViewModel
    {
        public DependantPropertyViewModel(string name, PropertyType editorType, INamedValueRepository tileConfiguration, INotifyPropertyChanged changeNotifier, Func<object[]> propertyFunc)
            : base(name, editorType, tileConfiguration, propertyFunc)
        {
            changeNotifier.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateParameters();
            OnPropertyChanged("Parameters");
        }
    }
}