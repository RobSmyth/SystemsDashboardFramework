using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using NoeticTools.SystemsDashboard.Framework.Annotations;
using NoeticTools.SystemsDashboard.Framework.Input;


namespace NoeticTools.SystemsDashboard.Framework.Adorners
{
    public class DropTargetAdorner : Adorner
    {
        private const double HintOpacity = 0.6;
        private const double MinimumThickness = 20.0;
        private static readonly Color FillColour = Colors.White;
        private static readonly Color LineColour = Colors.Black;
        private Action<DrawingContext, Rect, SolidColorBrush, Pen, double> _drawHandler;
        private RelativeDropPostion _relativeDropPostion;

        public DropTargetAdorner([NotNull] FrameworkElement adornedElement) : base(adornedElement)
        {
            Focusable = false;
            IsHitTestVisible = false;
            _relativeDropPostion = RelativeDropPostion.None;
            _drawHandler = NullDrawHandler;
        }

        private void NullDrawHandler(DrawingContext arg1, Rect arg2, SolidColorBrush arg3, Pen arg4, double arg5)
        {
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

        public void SetDropPosition(RelativeDropPostion relativeDropPostion)
        {
            if (_relativeDropPostion == relativeDropPostion)
            {
                return;
            }

            _relativeDropPostion = relativeDropPostion;

            var lookup = new Dictionary<RelativeDropPostion, Action<DrawingContext, Rect, SolidColorBrush, Pen, double>>
            {
                {RelativeDropPostion.None, NullDrawHandler},
                {RelativeDropPostion.Below, DrawBottomInsertHint},
                {RelativeDropPostion.ToLeft, DrawLeftInsertHint},
                {RelativeDropPostion.ToRight, DrawRightInsertHint},
                {RelativeDropPostion.Above, DrawTopInsertHint},
                {RelativeDropPostion.OnTop, DrawInsertDockHint},
                {RelativeDropPostion.TopHalf, DrawInsertDockHint},
                {RelativeDropPostion.BottomHalf, DrawInsertDockHint},
                {RelativeDropPostion.LeftHalf, DrawInsertDockHint},
                {RelativeDropPostion.RightHalf, DrawInsertDockHint},
            };

            _drawHandler = lookup[relativeDropPostion];

            InvalidateVisual();
        }

        private static void DrawBottomInsertHint(DrawingContext drawingContext, Rect adornedElementRect, SolidColorBrush renderBrush, Pen renderPen, double thickness)
        {
            var cornerA = adornedElementRect.BottomLeft;
            var cornerB = adornedElementRect.BottomRight;
            cornerA.Offset(0, 1);
            cornerB.Offset(0, 1);

            var streamGeometry = GetHorizontalInsertGeometry(cornerA, cornerB, thickness);
            drawingContext.DrawGeometry(renderBrush, renderPen, streamGeometry);
        }

        private static void DrawTopInsertHint(DrawingContext drawingContext, Rect adornedElementRect, SolidColorBrush renderBrush, Pen renderPen, double thickness)
        {
            var cornerA = adornedElementRect.TopLeft;
            var cornerB = adornedElementRect.TopRight;
            cornerA.Offset(0, -1);
            cornerB.Offset(0, -1);

            var streamGeometry = GetHorizontalInsertGeometry(cornerA, cornerB, thickness);
            drawingContext.DrawGeometry(renderBrush, renderPen, streamGeometry);
        }

        private void DrawInsertDockHint(DrawingContext drawingContext, Rect adornedElementRect, SolidColorBrush renderBrush, Pen renderPen, double thickness)
        {
            var smallestSize = Math.Min(adornedElementRect.Width, adornedElementRect.Height);

            var dropHintSize = new Size(smallestSize/4.0, smallestSize/4.0);
            var padding = smallestSize/20.0;

            var onTopHintRect = new Rect(dropHintSize);
            onTopHintRect.Offset(-onTopHintRect.Width/2.0, -onTopHintRect.Height/2.0);
            onTopHintRect.Offset(adornedElementRect.Width/2.0, adornedElementRect.Height/2.0);

            {
                var highlight = _relativeDropPostion == RelativeDropPostion.OnTop;
                DrawRectangle(drawingContext, renderPen, onTopHintRect, false);
                if (highlight)
                {
                    DrawRectangle(drawingContext, renderPen, adornedElementRect, true);
                }
            }

            {
                var highlight = _relativeDropPostion == RelativeDropPostion.TopHalf;
                var rect = new Rect(onTopHintRect.Left, onTopHintRect.Top - dropHintSize.Height - padding, dropHintSize.Width, dropHintSize.Height);
                DrawRectangle(drawingContext, renderPen, rect, false);
                if (highlight)
                {
                    DrawRectangle(drawingContext, renderPen, 
                        new Rect(adornedElementRect.TopLeft, 
                        new Size(adornedElementRect.Width, adornedElementRect.Height / 2.0)), true);
                }
            }

            {
                var highlight = _relativeDropPostion == RelativeDropPostion.BottomHalf;
                var rect = new Rect(onTopHintRect.Left, onTopHintRect.Bottom + padding, dropHintSize.Width, dropHintSize.Height);
                DrawRectangle(drawingContext, renderPen, rect, false);
                if (highlight)
                {
                    DrawRectangle(drawingContext, renderPen, 
                        new Rect(new Point(adornedElementRect.Left, adornedElementRect.Top + adornedElementRect.Height/2.0), 
                        new Size(adornedElementRect.Width, adornedElementRect.Height / 2.0)), true);
                }
            }

            {
                var highlight = _relativeDropPostion == RelativeDropPostion.LeftHalf;
                var rect = new Rect(onTopHintRect.Left - dropHintSize.Width - padding, onTopHintRect.Top, dropHintSize.Width, dropHintSize.Height);
                DrawRectangle(drawingContext, renderPen, rect, false);
                if (highlight)
                {
                    DrawRectangle(drawingContext, renderPen, 
                        new Rect(adornedElementRect.TopLeft, 
                        new Size(adornedElementRect.Width / 2.0, adornedElementRect.Height)), true);
                }
            }

            {
                var highlight = _relativeDropPostion == RelativeDropPostion.RightHalf;
                var rect = new Rect(onTopHintRect.Right + padding, onTopHintRect.Top, dropHintSize.Width, dropHintSize.Height);
                DrawRectangle(drawingContext, renderPen, rect, false);
                if (highlight)
                {
                    DrawRectangle(drawingContext, renderPen, 
                        new Rect(new Point(adornedElementRect.Left + adornedElementRect.Width / 2.0, adornedElementRect.Top),
                        new Size(adornedElementRect.Width / 2.0, adornedElementRect.Height)), true);
                }
            }
        }

        private void DrawRectangle(DrawingContext drawingContext, Pen renderPen, Rect rect, bool highlight)
        {
            var brush = new SolidColorBrush(FillColour) { Opacity = highlight ? HintOpacity : HintOpacity / 4.0 };
            drawingContext.DrawRectangle(brush, renderPen, rect);
        }

        private static void DrawLeftInsertHint(DrawingContext drawingContext, Rect adornedElementRect, SolidColorBrush renderBrush, Pen renderPen, double thickness)
        {
            renderBrush.Opacity = HintOpacity;
            var cornerA = adornedElementRect.TopLeft;
            var cornerB = adornedElementRect.BottomLeft;
            cornerA.Offset(-1, 0);
            cornerB.Offset(-1, 0);

            var streamGeometry = GetVerticalInsertGeometryx(cornerA, cornerB, thickness);
            drawingContext.DrawGeometry(renderBrush, renderPen, streamGeometry);
        }

        private static void DrawRightInsertHint(DrawingContext drawingContext, Rect adornedElementRect, SolidColorBrush renderBrush, Pen renderPen, double thickness)
        {
            renderBrush.Opacity = HintOpacity;
            var cornerA = adornedElementRect.TopRight;
            var cornerB = adornedElementRect.BottomRight;
            cornerA.Offset(1, 0);
            cornerB.Offset(1, 0);

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
                    new Point(top.X + thickness/2, top.Y + thickness/2)
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
                    new Point(left.X + thickness/2, right.Y + thickness/2)
                };
                geometryContext.PolyLineTo(points, true, true);
            }
            return streamGeometry;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var adornedElementRect = new Rect(AdornedElement.RenderSize);
            var renderBrush = new SolidColorBrush(FillColour) {Opacity = HintOpacity };
            var renderPen = new Pen(new SolidColorBrush(LineColour) {Opacity = 0.6}, 1.5);
            var thickness = (_relativeDropPostion & RelativeDropPostion.Horizontal) != 0 ? adornedElementRect.Width : adornedElementRect.Height;
            thickness = Math.Max(MinimumThickness, thickness/20.0);
            _drawHandler(drawingContext, adornedElementRect, renderBrush, renderPen, thickness);
        }
    }
}