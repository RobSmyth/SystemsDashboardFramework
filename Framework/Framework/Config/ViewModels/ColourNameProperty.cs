using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public sealed class ColourNameProperty : ITextProperty, IComparable
    {
        public ColourNameProperty(string name)
        {
            Text = name;
            Image = GetRectangle(name);
        }

        public FrameworkElement Image { get; }
        public string Text { get; }

        private static FrameworkElement GetRectangle(string colourName)
        {
            var color = (Color) ColorConverter.ConvertFromString(colourName);
            var rect = new Rectangle
            {
                Width = 14,
                Height = 14,
                Margin = new Thickness(1),
                Fill = new SolidColorBrush(color),
                Stroke = new SolidColorBrush(Colors.Black),
            };
            return rect;
        }

        public int CompareTo(object obj)
        {
            var otherProperty = obj as ITextProperty;
            if (otherProperty != null)
            {
                return Text.CompareTo(otherProperty.Text);
            }
            return GetHashCode().CompareTo(obj.GetHashCode());
        }

        public override string ToString()
        {
            return Text;
        }
    }
}