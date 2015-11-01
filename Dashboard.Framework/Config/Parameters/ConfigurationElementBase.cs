using System.Windows.Controls;


namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class ConfigurationElementBase
    {
        protected static int AddRow(Grid parametersGrid)
        {
            var rowIndex = parametersGrid.RowDefinitions.Count;
            parametersGrid.RowDefinitions.Add(new RowDefinition());
            return rowIndex;
        }
    }
}