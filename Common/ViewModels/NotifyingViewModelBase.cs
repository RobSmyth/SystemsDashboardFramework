using System.ComponentModel;
using System.Runtime.CompilerServices;
using NoeticTools.TeamStatusBoard.Common.Annotations;


namespace NoeticTools.TeamStatusBoard.Common.ViewModels
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