using System;


namespace NoeticTools.TeamStatusBoard.Common
{
    public interface IEventBroadcaster
    {
        void AddListener(object listener, Action callback);
        void RemoveListener(object listener);
        void Fire();
    }
}