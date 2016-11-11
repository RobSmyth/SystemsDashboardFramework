using System;


namespace NoeticTools.TeamStatusBoard.Framework.Services.TimeServices
{
    public class Clock : IClock
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}