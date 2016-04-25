using System;
using System.ComponentModel;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class DependantPropertyViewModel : PropertyViewModel
    {
        public DependantPropertyViewModel(string name, string viewerName, TileConfigurationConverter tileConfigurationConverter, INotifyPropertyChanged changeNotifier, Func<object[]> propertyFunc)
            : base(name, viewerName, tileConfigurationConverter, propertyFunc)
        {
            changeNotifier.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Parameters");
        }
    }
}