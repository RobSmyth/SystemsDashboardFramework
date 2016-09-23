using System;
using System.Windows;


namespace NoeticTools.TeamStatusBoard.Framework.Styles
{
    public sealed class StatusBoardStyleStategy
    {
        private readonly FrameworkElement _element;
        private readonly IStatusBoardStyle _style;

        public StatusBoardStyleStategy(FrameworkElement element, IStatusBoardStyle style)
        {
            _element = element;
            _style = style;
            _style.Broadcaster.AddListener(this, Set);
            Set();
        }

        private void Set()
        {
            _element.Resources.MergedDictionaries.Clear();
            _element.Resources.MergedDictionaries.Add(new ResourceDictionary{Source = new Uri(_style.StyleUrl, UriKind.Relative)});
        }
    }
}