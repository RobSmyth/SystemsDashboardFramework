using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;


namespace NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories
{
    public sealed class NullValueRepository : INamedValueRepository
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
            try
            {
                return bool.Parse(name);
            }
            catch (Exception)
            {
                return !string.IsNullOrWhiteSpace(name);
            }
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

        public Color GetColour(string name, string defaultValue)
        {
            try
            {
                return (Color)ColorConverter.ConvertFromString(name);
            }
            catch (Exception)
            {
                return (Color)ColorConverter.ConvertFromString(defaultValue);
            }
        }

        public double[] GetDoubleArray(string name)
        {
            return GetStringArray(name).Select(x => GetDouble(x)).ToArray();
        }

        public string[] GetStringArray(string name)
        {
            return name.Split(',');
        }

        public Color[] GetColourArray(string name)
        {
            return GetStringArray(name).Select(x => GetColour(x, "White")).ToArray();
        }
    }
}