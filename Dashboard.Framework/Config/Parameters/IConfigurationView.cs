using System.Windows.Controls;

namespace Dashboard.Config.Parameters
{
    public interface IConfigurationView
    {
        void Show(Grid parametersGrid, TileConfiguration tileConfiguration);
        void Save(Grid parametersGrid);
    }
}