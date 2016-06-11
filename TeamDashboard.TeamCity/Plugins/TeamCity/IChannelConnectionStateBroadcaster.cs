﻿using NoeticTools.TeamStatusBoard.Framework;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public interface IChannelConnectionStateBroadcaster
    {
        EventBroadcaster OnConnected { get; }
        EventBroadcaster OnDisconnected { get; }
    }
}