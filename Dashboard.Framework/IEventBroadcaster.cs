using System;


namespace NoeticTools.TeamStatusBoard.Framework
{
    public interface IEventBroadcaster
    {
        void AddListener(object listener, Action callback);
        void RemoveListener(object listener);
        void Flush();
        void Fire();
    }
}