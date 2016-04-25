using System.Windows;


namespace NoeticTools.TeamStatusBoard.Framework.Input
{
    public interface IDragSource
    {
        UIElement Element { get; }
        void OnMouseDragStarted();
    }
}