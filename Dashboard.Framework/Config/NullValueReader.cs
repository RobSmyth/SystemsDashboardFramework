using System;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public sealed class NullValueReader : INamedValueReader
    {
        public DateTime GetDateTime(string name)
        {
            return DateTime.Parse(name);
        }

        public TimeSpan GetTimeSpan(string name)
        {
            return TimeSpan.Parse(name);
        }

        public string GetString(string name)
        {
            return name;
        }

        public string GetString(string name, string defaultValue)
        {
            return name;
        }

        public double GetDouble(string name, double defaultValue = 0)
        {
            double value;
            if (double.TryParse(name, out value))
            {
                return value;
            }
            return defaultValue;
        }

        public bool GetBool(string name)
        {
            return bool.Parse(name);
        }

        public object GetParameter(string name)
        {
            return name;
        }

        public object GetParameter(string name, object defaultValue)
        {
            return name ?? defaultValue;
        }

        public void SetParameter(string name, object value)
        {
            throw new InvalidOperationException();
        }
    }
}