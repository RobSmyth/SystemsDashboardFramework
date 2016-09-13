using System;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public interface INamedValueReader
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
    }
}