using System.Windows;
using Dashboard.Services.TeamCity;


namespace NoeticTools.Dashboard.Framework.DataSources.TeamCity
{
    internal class TeamCityConfigurationViewModel
    {
        private readonly TeamCityServiceConfiguration _configuration;
        private readonly TeamCityConfigurationView _view;

        public TeamCityConfigurationViewModel(TeamCityServiceConfiguration configuration, TeamCityConfigurationView view)
        {
            _configuration = configuration;
            _view = view;
            _view.DataContext = this;
            _view.OkButton.Click += OkButton_Click;
            _view.CancelButton.Click += CancelButton_Click;
            _view.Loaded += _view_Loaded;

            Url = _configuration.Url;
            UserName = _configuration.UserName;
        }

        public string Url { get; set; }
        public string UserName { get; set; }

        public void Show()
        {
            _view.DataContext = this;
            _view.Show();
        }

        private void _view_Loaded(object sender, RoutedEventArgs e)
        {
            _view.PasswordBox.Password = _configuration.Password;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _view.Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            _configuration.Url = Url;
            _configuration.UserName = UserName;
            _configuration.Password = _view.PasswordBox.Password;
            _view.Close();
        }
    }
}