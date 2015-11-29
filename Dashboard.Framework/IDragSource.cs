using System.Windows;


namespace NoeticTools.Dashboard.Framework
{
    public interface IDragSource
    {
        UIElement Element { get; }
        void OnMouseDragStarted();
    }
}