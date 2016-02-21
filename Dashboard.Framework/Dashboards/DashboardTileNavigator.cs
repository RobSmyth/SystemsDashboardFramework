using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace NoeticTools.SystemsDashboard.Framework.Dashboards
{
    public class DashboardTileNavigator : IDashboardTileNavigator
    {
        private readonly Panel _tilePanel;

        public DashboardTileNavigator(Panel tilePanel)
        {
            _tilePanel = tilePanel;
        }

        public void MoveRight()
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
        }

        public void MoveLeft()
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
        }

        public void MoveUp()
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
        }

        public void MoveDown()
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
        }

        public void MoveTo(UIElement target)
        {
            SetFocus(target);
        }

        private void MoveFocus(TraversalRequest request)
        {
            var focusedElement = Keyboard.FocusedElement as UIElement;
            if (!(focusedElement is UserControl))
            {
                _tilePanel.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            }
            else
            {
                focusedElement.MoveFocus(request);
            }
        }

        private static bool SetFocus(UIElement element)
        {
            if (element == null)
            {
                return false;
            }

            if (element.Focusable)
            {
                Keyboard.Focus(element);
                return true;
            }

            var contentControl = element as ContentControl;
            var child = contentControl?.Content as UIElement;
            if (child != null)
            {
                return SetFocus(child);
            }

            var panel = element as Panel;
            return panel != null && panel.Children.Cast<UIElement>().Any(SetFocus);
        }
    }
}