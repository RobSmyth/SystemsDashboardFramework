using System;
using System.Collections.Generic;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories
{
    public interface INamedValueRepository
    {
        DateTime GetDateTime(string name);
        TimeSpan GetTimeSpan(string name);
        string GetString(string name);
        string GetString(string name, string defaultValue);
        object GetParameter(string name);
        object GetParameter(string name, object defaultValue);
        void SetParameter(string name, object value);
        Color GetColour(string name, string defaultValue);
        string[] GetStringArray(string name);
        Color[] GetColourArray(string name);
        T Get<T>(string name);
        IDataValue GetDatum(string name, object defaultValue = null);
        IEnumerable<IDataValue> GetDatums(string name);
    }
}