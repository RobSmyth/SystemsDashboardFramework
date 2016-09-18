using System;
using System.Windows.Media;


namespace NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories
{
    public interface INamedValueRepository
    {
        DateTime GetDateTime(string name);
        TimeSpan GetTimeSpan(string name);
        string GetString(string name);
        string GetString(string name, string defaultValue);
        double GetDouble(string name, double defaultValue = 0.0);
        bool GetBool(string name);
        object GetParameter(string name);
        object GetParameter(string name, object defaultValue);
        void SetParameter(string name, object value);
        Color GetColour(string name, string defaultValue);
        double[] GetDoubleArray(string name);
        string[] GetStringArray(string name);
        Color[] GetColourArray(string name);
        T Get<T>(string name);
    }
}