using System;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories
{
    public class DataSourceNamedValueRepository : INamedValueRepository
    {
        private readonly IDataSource _dataService;

        public DataSourceNamedValueRepository(IDataSource dataService)
        {
            _dataService = dataService;
        }

        public DateTime GetDateTime(string name)
        {
            return _dataService.Read<DateTime>(GetPropertyName(name));
        }

        public TimeSpan GetTimeSpan(string name)
        {
            return _dataService.Read<TimeSpan>(GetPropertyName(name));
        }

        public string GetString(string name)
        {
            return _dataService.Read<string>(GetPropertyName(name));
        }

        public string GetString(string name, string defaultValue)
        {
            return _dataService.Read<string>(GetPropertyName(name));
        }

        public double GetDouble(string name, double defaultValue = 0)
        {
            return _dataService.Read<double>(GetPropertyName(name));
        }

        public bool GetBool(string name)
        {
            return _dataService.Read<bool>(GetPropertyName(name));
        }

        public object GetParameter(string name)
        {
            return _dataService.Read<object>(GetPropertyName(name));
        }

        public object GetParameter(string name, object defaultValue)
        {
            return _dataService.Read<object>(GetPropertyName(name));
        }

        public void SetParameter(string name, object value)
        {
            _dataService.Write(GetPropertyName(name), value);
        }

        public Color GetColour(string name, string defaultValue)
        {
            throw new NotImplementedException();
        }

        public double[] GetDoubleArray(string name)
        {
            throw new NotImplementedException();
        }

        public string[] GetStringArray(string name)
        {
            throw new NotImplementedException();
        }

        public Color[] GetColourArray(string name)
        {
            throw new NotImplementedException();
        }

        private string GetPropertyName(string name)
        {
            return name.Substring(_dataService.TypeName.Length + 2);
        }
    }
}