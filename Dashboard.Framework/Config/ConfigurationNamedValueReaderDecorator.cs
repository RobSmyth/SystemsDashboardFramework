using System;
using System.Windows.Media;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public class ConfigurationNamedValueReaderDecorator : INamedValueReader
    {
        private readonly INamedValueReader _configurationReader;
        private readonly INamedValueReader _reader;

        public ConfigurationNamedValueReaderDecorator(INamedValueReader configurationReader, INamedValueReader reader)
        {
            _configurationReader = configurationReader;
            _reader = reader;
        }

        public DateTime GetDateTime(string name)
        {
            return _reader.GetDateTime(GetValue(name));
        }

        public TimeSpan GetTimeSpan(string name)
        {
            return _reader.GetTimeSpan(GetValue(name));
        }

        public string GetString(string name)
        {
            return _reader.GetString(GetValue(name));
        }

        public string GetString(string name, string defaultValue)
        {
            return _reader.GetString(GetValue(name), defaultValue);
        }

        public double GetDouble(string name, double defaultValue = 0)
        {
            return _reader.GetDouble(GetValue(name), defaultValue);
        }

        public bool GetBool(string name)
        {
            return _reader.GetBool(GetValue(name));
        }

        public object GetParameter(string name)
        {
            return _reader.GetParameter(GetValue(name));
        }

        public object GetParameter(string name, object defaultValue)
        {
            return _reader.GetParameter(name, defaultValue);
        }

        public void SetParameter(string name, object value)
        {
            _configurationReader.SetParameter(name, value);
        }

        public Color GetColour(string name, string defaultValue)
        {
            return _reader.GetColour(GetValue(name), defaultValue);
        }

        private string GetValue(string name)
        {
            return _configurationReader.GetString(name);
        }
    }
}