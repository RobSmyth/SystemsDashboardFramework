using System.ComponentModel;
using System.Runtime.CompilerServices;
using NoeticTools.TeamStatusBoard.Framework.Annotations;

namespace NoeticTools.TeamStatusBoard.Framework
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