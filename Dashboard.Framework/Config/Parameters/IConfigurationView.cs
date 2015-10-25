using System.Windows.Controls;

namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public interface IConfigurationView
    {
        void Show(Grid parametersGrid, TileConfiguration tileConfiguration);
        void Save(Grid parametersGrid);
    }
}