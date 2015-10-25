using System;

namespace NoeticTools.Dashboard.Framework
{
    public interface IClock
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}