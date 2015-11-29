using System.Windows;


namespace NoeticTools.Dashboard.Framework.Input
{
    public interface IDragSource
    {
        UIElement Element { get; }
        void OnMouseDragStarted();
    }
}