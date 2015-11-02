using System.Windows.Controls;


namespace NoeticTools.Dashboard.Framework.Config
{
    public interface IConfigurationView
    {
        void Show(Grid parametersGrid, TileConfigurationConverter tileConfigurationConverter);
        void Save(Panel parametersPanel);
    }
}