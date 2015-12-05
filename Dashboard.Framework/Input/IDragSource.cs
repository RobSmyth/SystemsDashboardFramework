using System.Windows;


namespace NoeticTools.SystemsDashboard.Framework.Input
{
    public interface IDragSource
    {
        UIElement Element { get; }
        void OnMouseDragStarted();
    }
}