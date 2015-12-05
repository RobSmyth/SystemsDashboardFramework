using System;


namespace NoeticTools.SystemsDashboard.Framework
{
    public interface IClock
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}