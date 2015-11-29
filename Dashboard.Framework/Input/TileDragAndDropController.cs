using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Input
{
    public class TileDragAndDropController
    {
        private readonly IDictionary<object, TileLayoutController> _elementToLayoutController = new Dictionary<object, TileLayoutController>();
        private readonly IDictionary<object, TileConfiguration> _elementToConfiguration = new Dictionary<object, TileConfiguration>();
        private Point _mouseDownPoint;
        private IDragSource _potentialSender;

        public void RegisterTarget(UIElement targetUiElement, TileLayoutController tileLayoutController, TileConfiguration tileConfiguration)
        {
            _elementToLayoutController.Add(targetUiElement, tileLayoutController);
            _elementToConfiguration.Add(targetUiElement, tileConfiguration);

            targetUiElement.AllowDrop = true;
            targetUiElement.DragEnter += OnDragEnter;
            targetUiElement.DragOver += OnDragOver;
            targetUiElement.DragLeave += OnDragLeave;
            targetUiElement.Drop += OnDrop;
        }

        public void RegisterSource(IDragSource source)
        {
            source.Element.PreviewMouseLeftButtonDown += ProvidersList_OnPreviewMouseLeftButtonDown;
            source.Element.PreviewMouseMove += ProvidersList_OnPreviewMouseMove;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (IsTileProviderData(e))
            {
                ((UIElement) sender).Opacity = 0.7;
            }
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (IsTileProviderData(e))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            ((UIElement) sender).Opacity = 1.0;
        }

        private static bool IsTileProviderData(DragEventArgs e)
        {
            return e.Data.GetDataPresent(typeof (TileConfiguration));
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            var newTileConfiguration = e.Data.GetData(typeof (TileConfiguration)) as TileConfiguration;
            if (newTileConfiguration == null)
            {
                return;
            }

            var target = (UIElement) sender;
            var dropPostion = GetRelativeDropPostion(e, target);
            if (dropPostion == RelativeDropPostion.OnTop)
            {
                DropOnTop(target, newTileConfiguration);
            }
            else
            {
                InsertDroppedTile(dropPostion, target, newTileConfiguration);
            }
        }

        private void DropOnTop(UIElement target, TileConfiguration newTileConfiguration)
        {
            // todo - replace or make new group
        }

        private void InsertDroppedTile(RelativeDropPostion dropPostion, object sender, TileConfiguration newTile)
        {
            var layoutController = _elementToLayoutController[sender];
            var targetTile = _elementToConfiguration[sender];

            newTile.RowNumber = targetTile.RowNumber;
            newTile.ColumnNumber = targetTile.ColumnNumber;
            newTile.RowSpan = 1;
            newTile.ColumnSpan = 1;

            if (dropPostion == RelativeDropPostion.Right)
            {
                layoutController.InsertNewColumn(targetTile.ColumnNumber + targetTile.ColumnSpan);
            }
            else if (dropPostion == RelativeDropPostion.Left)
            {
                layoutController.InsertNewColumn(targetTile.ColumnNumber);
            }
            else if (dropPostion == RelativeDropPostion.Top)
            {
                layoutController.InsertNewRow(targetTile.RowNumber - 1);
            }
            else if (dropPostion == RelativeDropPostion.Bottom)
            {
                layoutController.InsertNewRow(targetTile.RowNumber + targetTile.ColumnSpan - 1);
            }

            layoutController.AddTile(newTile);
        }

        private static RelativeDropPostion GetRelativeDropPostion(DragEventArgs e, UIElement target)
        {
            var targetCenter = new Point(target.RenderSize.Width/2, target.RenderSize.Height/2);
            var dropPosition = e.GetPosition(target);
            dropPosition.Offset(-targetCenter.X, -targetCenter.Y);
            var normalisedPosition = new Point(dropPosition.X/(targetCenter.X*0.75), dropPosition.Y/(targetCenter.Y*0.75));

            if (Math.Abs(normalisedPosition.X) < Math.Abs(normalisedPosition.Y))
            {
                if (normalisedPosition.Y > 1.0)
                {
                    return RelativeDropPostion.Bottom;
                }
                if (normalisedPosition.Y < -1.0)
                {
                    return RelativeDropPostion.Top;
                }
            }
            else
            {
                if (normalisedPosition.X > 1.0)
                {
                    return RelativeDropPostion.Right;
                }
                if (normalisedPosition.X < -1.0)
                {
                    return RelativeDropPostion.Left;
                }
            }
            return RelativeDropPostion.OnTop;
        }

        private void ProvidersList_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _potentialSender = sender as IDragSource;
            _mouseDownPoint = e.GetPosition(null);
        }

        private void ProvidersList_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            var source = sender as IDragSource;
            if (source == null || !ReferenceEquals(_potentialSender, source))
            {
                _potentialSender = null;
                return;
            }

            var mousePos = e.GetPosition(null);
            var diff = _mouseDownPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                _potentialSender = null;
                source.OnMouseDragStarted();
            }
        }
    }
}