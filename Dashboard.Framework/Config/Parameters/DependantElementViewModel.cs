using System;
using System.ComponentModel;


namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class DependantElementViewModel : ElementViewModel
    {
        private readonly Action<IElementViewModel> _update;

        public DependantElementViewModel(string name, ElementType elementType, TileConfigurationConverter tileConfigurationConverter, INotifyPropertyChanged changeNotifier, Action<IElementViewModel> update)
            : base(name, elementType, tileConfigurationConverter)
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