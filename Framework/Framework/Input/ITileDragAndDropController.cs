using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Input
{
    public interface ITileDragAndDropController
    {
        void RegisterTarget(UIElement targetUiElement, ITileLayoutController tileLayoutController, TileConfiguration tileConfiguration);
        void Register(FrameworkElement source);
        void RegisterSource(IDragSource source);
        void DeRegister(UIElement view);
    }
}