using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Framework.Annotations;


namespace NoeticTools.TeamStatusBoard.Framework.Adorners
{
    public class GroupPanelDetailsAdorner : Adorner
    {
        private const int MarginIncrease = 4;
        private const int NameFontSize = 24;
        private readonly FrameworkElement _adornedElement;
        private readonly Thickness _originalMargin;
        private static readonly Color Colour = Colors.AntiqueWhite;
        private readonly Brush _nameBrush = Brushes.Black;

        public GroupPanelDetailsAdorner([NotNull] FrameworkElement adornedElement) : base(adornedElement)
        {
            _adornedElement = adornedElement;
            _originalMargin = _adornedElement.Margin;
        }

        public void Attach()
        {
            _adornedElement.Margin = new Thickness(_originalMargin.Left + MarginIncrease, _originalMargin.Top + MarginIncrease, _originalMargin.Right + MarginIncrease, _originalMargin.Bottom + MarginIncrease);
            var layer = AdornerLayer.GetAdornerLayer(AdornedElement);
            layer.Add(this);
        }

        public void Detach()
        {
            _adornedElement.Margin = _originalMargin;
            var layer = AdornerLayer.GetAdornerLayer(AdornedElement);
            layer.Remove(this);
        }

        private static void DrawCornerCircles(DrawingContext drawingContext, Rect adornedElementRect)
        {
            const double renderRadius = 5.0;
            var cornerNodeRenderBrush = new SolidColorBrush(Colour) {Opacity = 0.6};
            var cornerNodeRenderPen = new Pen(new SolidColorBrush(Colour), 1.5);
            drawingContext.DrawEllipse(cornerNodeRenderBrush, cornerNodeRenderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(cornerNodeRenderBrush, cornerNodeRenderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
            drawingContext.DrawEllipse(cornerNodeRenderBrush, cornerNodeRenderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(cornerNodeRenderBrush, cornerNodeRenderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var adornedElementRect = new Rect(AdornedElement.RenderSize);

            DrawOverlayingRectangle(drawingContext, adornedElementRect);
            DrawCornerCircles(drawingContext, adornedElementRect);

            DrawPanelId(drawingContext, adornedElementRect);
        }

        private void DrawPanelId(DrawingContext drawingContext, Rect adornedElementRect)
        {
            var textPoint = new Point(adornedElementRect.Left, adornedElementRect.Top);
            textPoint.Offset(10, 10);
            var typeface = new Typeface("Verdana");
            var formattedText = new FormattedText(_adornedElement.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, NameFontSize, _nameBrush);
            formattedText.SetFontWeight(FontWeights.Bold);
            formattedText.SetFontStyle(FontStyles.Italic);
            drawingContext.DrawText(formattedText, textPoint);
        }

        private static void DrawOverlayingRectangle(DrawingContext drawingContext, Rect adornedElementRect)
        {
            var renderBrush = new SolidColorBrush(Colour) {Opacity = 0.5};
            var renderPen = new Pen(new SolidColorBrush(Colour), 1.5);
            var rect = new Rect(adornedElementRect.TopLeft, adornedElementRect.BottomRight);
            drawingContext.DrawRectangle(renderBrush, renderPen, rect);
        }
    }
}