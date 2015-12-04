using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using NoeticTools.Dashboard.Framework.Annotations;
using NoeticTools.Dashboard.Framework.Input;


namespace NoeticTools.Dashboard.Framework.Adorners
{
    public class DropTargetAdorner : Adorner
    {
        private const double MinimumThickness = 20.0;
        private static readonly Color FillColour = Colors.White;
        private static readonly Color LineColour = Colors.Black;
        private Action<DrawingContext, Rect, SolidColorBrush, Pen, double> _drawHandler;
        private RelativeDropPostion _relativeDropPostion;

        public DropTargetAdorner([NotNull] FrameworkElement adornedElement) : base(adornedElement)
        {
            Focusable = false;
            IsHitTestVisible = false;
            _relativeDropPostion = RelativeDropPostion.OnTop;
            _drawHandler = DrawReplaceHint;
        }

        public void Attach()
        {
            var layer = AdornerLayer.GetAdornerLayer(AdornedElement);
            layer.Add(this);
        }

        public void Detach()
        {
            var layer = AdornerLayer.GetAdornerLayer(AdornedElement);
            layer.Remove(this);
        }

        private static void DrawBottomInsertHint(DrawingContext drawingContext, Rect adornedElementRect, SolidColorBrush renderBrush, Pen renderPen, double thickness)
        {
            renderBrush.Opacity = 0.7;
            var cornerA = adornedElementRect.BottomLeft;
            var cornerB = adornedElementRect.BottomRight;
            cornerA.Offset(0, 1);
            cornerB.Offset(0, 1);

            var streamGeometry = GetHorizontalInsertGeometry(cornerA, cornerB, thickness);
            drawingContext.DrawGeometry(renderBrush, renderPen, streamGeometry);
        }

        private static void DrawTopInsertHint(DrawingContext drawingContext, Rect adornedElementRect, SolidColorBrush renderBrush, Pen renderPen, double thickness)
        {
            renderBrush.Opacity = 0.7;
            var cornerA = adornedElementRect.TopLeft;
            var cornerB = adornedElementRect.TopRight;
            cornerA.Offset(0,-1);
            cornerB.Offset(0, -1);

            var streamGeometry = GetHorizontalInsertGeometry(cornerA, cornerB, thickness);
            drawingContext.DrawGeometry(renderBrush, renderPen, streamGeometry);
        }

        private static void DrawReplaceHint(DrawingContext drawingContext, Rect adornedElementRect, SolidColorBrush renderBrush, Pen renderPen, double thickness)
        {
            var streamGeometry = new StreamGeometry();
            using (var geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(adornedElementRect.TopLeft, true, true);
                var points = new PointCollection
                {
                    adornedElementRect.TopRight,
                    adornedElementRect.BottomRight,
                    adornedElementRect.BottomLeft,
                };
                geometryContext.PolyLineTo(points, true, true);
            }
            drawingContext.DrawGeometry(renderBrush, renderPen, streamGeometry);
        }

        private static void DrawLeftInsertHint(DrawingContext drawingContext, Rect adornedElementRect, SolidColorBrush renderBrush, Pen renderPen, double thickness)
        {
            renderBrush.Opacity = 0.7;
            var cornerA = adornedElementRect.TopLeft;
            var cornerB = adornedElementRect.BottomLeft;
            cornerA.Offset(-1,0);
            cornerB.Offset(-1,0);

            var streamGeometry = GetVerticalInsertGeometryx(cornerA, cornerB, thickness);
            drawingContext.DrawGeometry(renderBrush, renderPen, streamGeometry);
        }

        private static void DrawRightInsertHint(DrawingContext drawingContext, Rect adornedElementRect, SolidColorBrush renderBrush, Pen renderPen, double thickness)
        {
            renderBrush.Opacity = 0.7;
            var cornerA = adornedElementRect.TopRight;
            var cornerB = adornedElementRect.BottomRight;
            cornerA.Offset(1,0);
            cornerB.Offset(1,0);

            var streamGeometry = GetVerticalInsertGeometryx(cornerA, cornerB, thickness);
            drawingContext.DrawGeometry(renderBrush, renderPen, streamGeometry);
        }

        private static StreamGeometry GetVerticalInsertGeometryx(Point top, Point bottom, double thickness)
        {
            var streamGeometry = new StreamGeometry();
            using (var geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(top, true, true);
                var points = new PointCollection
                {
                    new Point(top.X - thickness/2, top.Y + thickness/2),
                    new Point(top.X - thickness/2, bottom.Y - thickness/2),
                    bottom,
                    new Point(top.X + thickness/2, bottom.Y - thickness/2),
                    new Point(top.X + thickness/2, top.Y + thickness/2),
                };
                geometryContext.PolyLineTo(points, true, true);
            }
            return streamGeometry;
        }

        private static StreamGeometry GetHorizontalInsertGeometry(Point left, Point right, double thickness)
        {
            var streamGeometry = new StreamGeometry();
            using (var geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(left, true, true);
                var points = new PointCollection
                {
                    new Point(left.X + thickness/2, left.Y - thickness/2),
                    new Point(right.X - thickness/2, right.Y - thickness/2),
                    right,
                    new Point(right.X - thickness/2, right.Y + thickness/2),
                    new Point(left.X + thickness/2, right.Y + thickness/2),
                };
                geometryContext.PolyLineTo(points, true, true);
            }
            return streamGeometry;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var adornedElementRect = new Rect(AdornedElement.RenderSize);
            var renderBrush = new SolidColorBrush(FillColour) { Opacity = 0.5 };
            var renderPen = new Pen(new SolidColorBrush(LineColour) {Opacity = 0.6}, 2.0);
            var thickness = (_relativeDropPostion & RelativeDropPostion.Horizontal) != 0 ? adornedElementRect.Width : adornedElementRect.Height;
            thickness = Math.Max(MinimumThickness, thickness / 20.0);
            _drawHandler(drawingContext, adornedElementRect, renderBrush, renderPen, thickness);
        }

        public void SetDropPosition(RelativeDropPostion relativeDropPostion)
        {
            if (_relativeDropPostion == relativeDropPostion)
            {
                return;
            }

            _relativeDropPostion = relativeDropPostion;

            var lookup = new Dictionary<RelativeDropPostion, Action<DrawingContext, Rect, SolidColorBrush, Pen, double>>
            {
                {RelativeDropPostion.Bottom, DrawBottomInsertHint},
                {RelativeDropPostion.Left, DrawLeftInsertHint},
                {RelativeDropPostion.Right, DrawRightInsertHint},
                {RelativeDropPostion.Top, DrawTopInsertHint},
                {RelativeDropPostion.OnTop, DrawReplaceHint},
            };

            _drawHandler = lookup[relativeDropPostion];

            InvalidateVisual();
        }
    }
}