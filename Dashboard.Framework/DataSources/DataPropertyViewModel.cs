using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace NoeticTools.SystemsDashboard.Framework.DataSources
{
    public class DataPropertyViewModel<T> : NotifyingViewModelBase, IDataPropertyViewModel<T>
    {
        private T _value;

        public T Value
        {
            get { return _value; }
            set
            {
                if (Equals(value, _value)) return;
                _value = value;
                OnPropertyChanged();
            }
        }
    }
}