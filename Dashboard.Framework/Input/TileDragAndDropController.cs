using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Adorners;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Input
{
    public sealed class TileDragAndDropController
    {
        private readonly IDictionary<object, ITileLayoutController> _elementToLayoutController = new Dictionary<object, ITileLayoutController>();
        private readonly IDictionary<object, TileConfiguration> _elementToTile = new Dictionary<object, TileConfiguration>();

        private readonly Dictionary<RelativeDropPostion, TileInsertAction> _insertActionMap = new Dictionary<RelativeDropPostion, TileInsertAction>
        {
            {RelativeDropPostion.Top, TileInsertAction.Above},
            {RelativeDropPostion.Bottom, TileInsertAction.Below},
            {RelativeDropPostion.Left, TileInsertAction.ToLeft},
            {RelativeDropPostion.Right, TileInsertAction.ToRight}
        };

        private Point _mouseDownPoint;
        private object _potentialSender;

        private UIElement _draggedElement;
        private DropTargetAdorner _dropHighlightAdorner;

        public void RegisterTarget(UIElement targetUiElement, ITileLayoutController tileLayoutController, TileConfiguration tileConfiguration)
        {
            _elementToLayoutController.Add(targetUiElement, tileLayoutController);
            _elementToTile.Add(targetUiElement, tileConfiguration);

            targetUiElement.AllowDrop = true;
            targetUiElement.DragEnter += OnDragEnter;
            targetUiElement.DragOver += OnDragOver;
            targetUiElement.DragLeave += OnDragLeave;
            targetUiElement.Drop += OnDrop;
        }

        public void Register(FrameworkElement source)
        {
            source.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            source.PreviewMouseLeftButtonUp += Source_PreviewMouseLeftButtonUp;
            source.PreviewMouseMove += OnPreviewMouseMove;
        }

        public void RegisterSource(IDragSource source)
        {
            source.Element.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            source.Element.PreviewMouseMove += OnPreviewMouseMove;
        }

        public void DeRegister(UIElement view)
        {
            _elementToLayoutController.Remove(view);
            _elementToTile.Remove(view);
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (ReferenceEquals(sender, _draggedElement))
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            if (IsTileProviderData(e))
            {
                _dropHighlightAdorner = new DropTargetAdorner((FrameworkElement) sender);
                _dropHighlightAdorner.Attach();
            }
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (ReferenceEquals(sender, _draggedElement))
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            if (IsTileProviderData(e))
            {
                var relativeDropPostion = GetRelativeDropPostion(e, (UIElement) sender);
                e.Effects = DragDropEffects.All;
                _dropHighlightAdorner.SetDropPosition(relativeDropPostion);
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            var element = (UIElement) sender;
            OnDragLeave(element);
        }

        private void OnDragLeave(UIElement element)
        {
            if (ReferenceEquals(element, _draggedElement))
            {
                return;
            }

            _dropHighlightAdorner.Detach();
            _dropHighlightAdorner = null;
        }

        private static bool IsTileProviderData(DragEventArgs e)
        {
            return e.Data.GetDataPresent(typeof (TileConfiguration));
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (ReferenceEquals(sender, _draggedElement))
            {
                return;
            }

            var newTileConfiguration = e.Data.GetData(typeof (TileConfiguration)) as TileConfiguration;
            if (newTileConfiguration == null)
            {
                return;
            }

            var target = (UIElement) sender;
            OnDragLeave(target);

            var dropPostion = GetRelativeDropPostion(e, target);

            if (e.Effects == DragDropEffects.Copy)
            {
                Copy(dropPostion, target, newTileConfiguration);
            }
            else if (e.Effects == DragDropEffects.Move)
            {
                Move(dropPostion, target, newTileConfiguration);
            }
        }

        private void Move(RelativeDropPostion dropPostion, UIElement target, TileConfiguration newTileConfiguration)
        {
            if (dropPostion == RelativeDropPostion.OnTop)
            {
                ReplaceWith(target, newTileConfiguration);
            }
            else
            {
                InsertMoved(dropPostion, target, newTileConfiguration);
            }
        }

        private void InsertMoved(RelativeDropPostion dropPostion, UIElement target, TileConfiguration tileBingMoved)
        {
            tileBingMoved.SetLocation(_elementToTile[target]);
            Remove(tileBingMoved);
            InsertCopy(dropPostion, target, tileBingMoved);
        }

        private void ReplaceWith(UIElement target, TileConfiguration tileBingMoved)
        {
            tileBingMoved.SetLocation(_elementToTile[target]);
            Remove(tileBingMoved);
            var targetLayoutController = _elementToLayoutController[target];
            targetLayoutController.Replace(_elementToTile[target], tileBingMoved);
            _elementToTile.Remove(target);
        }

        private void Copy(RelativeDropPostion dropPostion, UIElement target, TileConfiguration newTile)
        {
            if (dropPostion == RelativeDropPostion.OnTop)
            {
                _elementToLayoutController[target].Replace(_elementToTile[target], newTile);
            }
            else
            {
                InsertCopy(dropPostion, target, newTile);
            }
        }

        private void InsertCopy(RelativeDropPostion dropPostion, object sender, TileConfiguration newTile)
        {
            var layoutController = _elementToLayoutController[sender];
            var targetTile = _elementToTile[sender];
            layoutController.InsertTile(newTile, _insertActionMap[dropPostion], targetTile);
        }

        private void Remove(TileConfiguration tileBingMoved)
        {
            var elementBeingMoved = _elementToTile.Single(x => x.Value.Equals(tileBingMoved)).Key;
            _elementToTile.Remove(elementBeingMoved);
            _elementToLayoutController[elementBeingMoved].Remove(tileBingMoved);
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

        private void Source_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _draggedElement = null;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _draggedElement = null;
            _potentialSender = sender;
            _mouseDownPoint = e.GetPosition(null);
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!ReferenceEquals(_potentialSender, sender))
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
                _draggedElement = (UIElement) _potentialSender;
                _potentialSender = null;

                var source = sender as IDragSource;
                if (source != null)
                {
                    StartDrag(source);
                }
                else
                {
                    var sourceElement = (FrameworkElement) sender;
                    var dragData = new DataObject(typeof (TileConfiguration), _elementToTile[sender]);
                    DragDrop.DoDragDrop(sourceElement, dragData, DragDropEffects.Move);
                }
            }
        }

        private void StartDrag(IDragSource source)
        {
            _potentialSender = null;
            source.OnMouseDragStarted();
        }
    }
}