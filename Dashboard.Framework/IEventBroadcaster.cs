using System;


namespace NoeticTools.TeamStatusBoard.Framework
{
    public interface IEventBroadcaster
    {
        void AddListener(Action callback);
        void RemoveListener(Action callback);
        void Flush();
        void Fire();
    }
}