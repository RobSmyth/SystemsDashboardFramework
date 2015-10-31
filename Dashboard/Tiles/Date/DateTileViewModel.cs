using System;
using System.Windows.Controls;
using System.Windows.Threading;
using NoeticTools.TeamDashboard.Tiles;

namespace Dashboard.Tiles.Date
{
    internal class DateTileViewModel : ITileViewModel
    {
        public static readonly string TileTypeId = "Date.Now";
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);
        private readonly DispatcherTimer _timer;
        private DateTileControl _view;

        public DateTileViewModel(Guid id)
        {
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = _tickPeriod;
            Id = id;
        }

        public void Start(Panel placeholderPanel)
        {
            _view = new DateTileControl();
            placeholderPanel.Children.Add(_view);
            UpdateView();
            _timer.Start();
        }

        public string TypeId
        {
            get { return TileTypeId; }
        }

        public Guid Id { get; private set; }

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