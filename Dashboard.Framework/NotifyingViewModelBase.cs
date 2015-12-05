using System.ComponentModel;
using System.Runtime.CompilerServices;
using NoeticTools.SystemsDashboard.Framework.Annotations;


namespace NoeticTools.SystemsDashboard.Framework
{
    public abstract class NotifyingViewModelBase : INotifyPropertyChanged
    {
        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}