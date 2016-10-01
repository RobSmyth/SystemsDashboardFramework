using System.Windows;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public interface ITextProperty
    {
        FrameworkElement Image { get; }
        string Text { get; }
    }
}