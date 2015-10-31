using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using NoeticTools.Dashboard.Framework.Annotations;

namespace NoeticTools.Dashboard.Framework.Config.ViewModels
{
    internal abstract class NotifyingViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}