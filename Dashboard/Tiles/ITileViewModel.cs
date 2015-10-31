using System;
using System.Windows.Controls;
using Dashboard.Config;

namespace NoeticTools.TeamDashboard.Tiles
{
    public interface ITileViewModel : IConfigurationChangeListener
    {
        string TypeId { get; }
        Guid Id { get; }
        void Start(Panel placeholderPanel);
    }
}