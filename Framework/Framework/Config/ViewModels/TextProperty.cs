using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public abstract class TextProperty : ITextProperty, IComparable
    {
        protected TextProperty(string text, string iconText, Brush iconBackground, Brush iconForeground)
        {
            Text = text;
            Image = new TextBlock
            {
                Text = iconText,
                Background = iconBackground,
                Foreground = iconForeground,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 16,
                TextAlignment = TextAlignment.Center,
            };
        }

        public FrameworkElement Image { get; }
        public string Text { get; }

        public int CompareTo(object obj)
        {
            var otherProperty = obj as ITextProperty;
            if (otherProperty != null)
            {
                return Text.CompareTo(otherProperty.Text);
            }
            return GetHashCode().CompareTo(obj.GetHashCode());
        }
    }
}