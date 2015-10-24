using System;
using System.Windows.Controls;
using Dashboard.Config;

namespace Dashboard.Tiles
{
    public interface ITileViewModel : IConfigurationChangeListener
    {
        Guid TypeId { get; }
        Guid Id { get; }
        void Start(Panel placeholderPanel);
    }
}