using System;
using System.ComponentModel;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;


namespace NoeticTools.Dashboard.Framework.Tiles.TeamCityLastBuildStatus
{
    public class TeamCityConfigurationElementViewModel : ElementViewModel
    {
        private readonly TeamCityService _service;
        private readonly INotifyingElementViewModel _projectElementViewModel;
        private string _projectName;

        public TeamCityConfigurationElementViewModel(string name, TileConfigurationConverter tileConfigurationConverter, TeamCityService service, INotifyingElementViewModel projectElementViewModel)
            : base(name, ElementType.SelectedText, tileConfigurationConverter)
        {
            _service = service;
            _projectElementViewModel = projectElementViewModel;
            _projectName = (string) projectElementViewModel.Value;
            Update();
            projectElementViewModel.PropertyChanged += OnPropertyChanged;
        }

        private void Update()
        {
            Parameters = _service.GetConfigurationNames(_projectName);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Value", StringComparison.InvariantCulture))
            {
                _projectName = (string)_projectElementViewModel.Value;
                Update();
            }
        }
    }
}