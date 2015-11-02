using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using NoeticTools.Dashboard.Framework.Annotations;


namespace NoeticTools.Dashboard.Framework.Adorners
{
    public class GroupTileHighlightAdorner : Adorner
    {
        private readonly FrameworkElement _adornedElement;
        private readonly Thickness _originalMargin;
        private const int MarginIncrease = 4;

        public GroupTileHighlightAdorner([NotNull] FrameworkElement adornedElement) : base(adornedElement)
        {
            _adornedElement = adornedElement;
            _originalMargin = _adornedElement.Margin;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var adornedElementRect = new Rect(AdornedElement.RenderSize);

            var renderBrush = new SolidColorBrush(Colors.AntiqueWhite) {Opacity = 0.5};
            var renderPen = new Pen(new SolidColorBrush(Colors.AntiqueWhite), 1.5);
            const double renderRadius = 5.0;

            var rect = new Rect(adornedElementRect.TopLeft, adornedElementRect.BottomRight);
            drawingContext.DrawRectangle(renderBrush, renderPen, rect);

            var cornerNodeRenderBrush = new SolidColorBrush(Colors.AntiqueWhite) { Opacity = 0.6 };
            var cornerNodeRenderPen = new Pen(new SolidColorBrush(Colors.AntiqueWhite), 1.5);
            drawingContext.DrawEllipse(cornerNodeRenderBrush, cornerNodeRenderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(cornerNodeRenderBrush, cornerNodeRenderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
            drawingContext.DrawEllipse(cornerNodeRenderBrush, cornerNodeRenderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(cornerNodeRenderBrush, cornerNodeRenderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
        }

        public void Attach()
        {
            _adornedElement.Margin = new Thickness(_originalMargin.Left+ MarginIncrease, _originalMargin.Top+ MarginIncrease, _originalMargin.Right+ MarginIncrease, _originalMargin.Bottom+ MarginIncrease);
            var layer = AdornerLayer.GetAdornerLayer(AdornedElement);
            layer.Add(this);
        }

        public void Detach()
        {
            _adornedElement.Margin = _originalMargin;
            var layer = AdornerLayer.GetAdornerLayer(AdornedElement);
            layer.Remove(this);
        }
    }
}