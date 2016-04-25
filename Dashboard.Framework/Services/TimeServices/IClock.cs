using System;


namespace NoeticTools.TeamStatusBoard.Framework.Services.TimeServices
{
    public interface IClock
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}