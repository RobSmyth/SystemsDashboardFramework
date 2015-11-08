using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config.Parameters;


namespace NoeticTools.Dashboard.Framework.Config
{
    public interface IElementViewModel : IConfigurationParameter
    {
        object[] Parameters { get; }
        object Value { get; set; }
    }
}