using System;
using System.ComponentModel;


namespace NoeticTools.Dashboard.Framework.Config.Properties
{
    public class DependantPropertyViewModel : PropertyViewModel
    {
        private readonly Action<IPropertyViewModel> _update;

        public DependantPropertyViewModel(string name, string viewerName, TileConfigurationConverter tileConfigurationConverter, INotifyPropertyChanged changeNotifier, Action<IPropertyViewModel> update)
            : base(name, viewerName, tileConfigurationConverter)
        {
            _update = update;
            Update();
            changeNotifier.PropertyChanged += OnPropertyChanged;
        }

        private void Update()
        {
            _update(this);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Update();
        }
    }
}