using System;
using System.Windows;
using System.Windows.Threading;

namespace NoeticTools.Dashboard.Framework.Tiles.Date
{
    internal class DateTileViewModel : ITileViewModel
    {
        public static readonly string TileTypeId = "Date.Now";
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);
        private readonly DispatcherTimer _timer;
        private DateTileControl _view;

        public DateTileViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = _tickPeriod;
        }

        public FrameworkElement CreateView()
        {
            _view = new DateTileControl();
            UpdateView();
            _timer.Start();
            return _view;
        }

        public void OnConfigurationChanged()
        {
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            UpdateView();
            _timer.Start();
        }

        private void UpdateView()
        {
            DateTime now = DateTime.Now;
            _view.day.Text = now.Day.ToString();
            _view.month.Text = now.ToString("MMM");
        }
    }
}