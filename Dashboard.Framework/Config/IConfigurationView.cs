using System.Windows.Controls;


namespace NoeticTools.Dashboard.Framework.Config
{
    public interface IConfigurationView
    {
        void Show(Grid parametersGrid, TileConfiguration tileConfiguration);
        void Save(Panel parametersPanel);
    }
}