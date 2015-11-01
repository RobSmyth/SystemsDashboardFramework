using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace NoeticTools.Dashboard.Framework
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
    }
}