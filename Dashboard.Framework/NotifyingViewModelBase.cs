using System.ComponentModel;
using System.Runtime.CompilerServices;
using NoeticTools.Dashboard.Framework.Annotations;


namespace NoeticTools.Dashboard.Framework
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