using System;


namespace NoeticTools.SystemsDashboard.Framework.Services.TimeServices
{
    public interface IClock
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}