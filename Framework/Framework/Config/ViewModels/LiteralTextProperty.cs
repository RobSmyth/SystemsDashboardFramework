using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public sealed class LiteralTextProperty : TextProperty
    {
        public LiteralTextProperty(string text) : base(text, "l", Brushes.LightBlue, Brushes.Black)
        {
        }
    }
}