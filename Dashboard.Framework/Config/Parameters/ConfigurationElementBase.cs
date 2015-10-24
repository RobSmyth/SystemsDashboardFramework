﻿using System.Windows.Controls;

namespace Dashboard.Config.Parameters
{
    public class ConfigurationElementBase
    {
        protected static int AddRow(Grid parametersGrid)
        {
            int rowIndex = parametersGrid.RowDefinitions.Count;
            parametersGrid.RowDefinitions.Add(new RowDefinition());
            return rowIndex;
        }
    }
}