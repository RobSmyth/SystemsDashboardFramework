using System;
using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.Dashboard.Framework.DataSources.Jira
{
    public class TimeCachedArray<T>
    {
        private readonly Func<IEnumerable<T>> _loader;
        private readonly TimeSpan _lifeTime;
        private readonly IClock _clock;
        private DateTime _nextRefresh;
        private T[] _items = new T[0];

        public TimeCachedArray(Func<IEnumerable<T>> loader, TimeSpan lifeTime, IClock clock)
        {
            _loader = loader;
            _lifeTime = lifeTime;
            _clock = clock;
            _nextRefresh = DateTime.MinValue;
        }

        public T[] Items
        {
            get
            {
                var now = _clock.UtcNow;
                if (now >= _nextRefresh)
                {
                    _nextRefresh = now.Add(TimeSpan.FromMinutes(5));
                    try
                    {
                        _items = _loader().ToArray();
                    }
                    finally
                    {
                        _nextRefresh = now.Add(_lifeTime);
                    }
                }
                return _items;
            }
        }
    }
}