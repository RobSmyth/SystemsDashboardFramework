using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Input
{
    public interface ITileDragAndDropController
    {
        void RegisterTarget(UIElement targetUiElement, ITileLayoutController tileLayoutController, TileConfiguration tileConfiguration);
        void Register(FrameworkElement source);
        void RegisterSource(IDragSource source);
        void DeRegister(UIElement view);
    }
}