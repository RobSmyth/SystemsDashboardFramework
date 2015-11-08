using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config.Parameters;


namespace NoeticTools.Dashboard.Framework.Config
{
    public interface IConfigurationElement : IConfigurationParameter
    {
        void Save(Panel parametersPanel);
        object[] Parameters { get; }
    }
}