using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts.Wpf;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Tiles.Guages.GuageAngular
{
    internal sealed class GuageAngularTileViewModel : ConfiguredTileViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private const double DefaultTickStepRatio = 4.0;
        private const double DefaultLabelsStepRatio = 2.0;
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private IDataValue _value = new NullDataValue();
        private IDataValue _label = new NullDataValue();
        private IDataValue _maximum = new NullDataValue(100.0);
        private IDataValue _minimum = new NullDataValue();
        private IDataValue _labelsStep = new NullDataValue();
        private IDataValue _ticksStep = new NullDataValue();
        private string _format = "{0}";

        public GuageAngularTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, ITileProperties properties)
            : base(properties)
        {
            _services = services;

            var parameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "Data Value Tile Configuration", parameters, dashboardController, layoutController, services);

            Subscribe();

            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        private void Subscribe()
        {
            _label = UpdateSubscription(_label, "Label", "Label");
            _minimum = UpdateSubscription(_minimum, "Minimum", 0.0);
            _maximum = UpdateSubscription(_maximum, "Maximum", 100.0);
            _value = UpdateSubscription(_value, "Value", 0.0);
            _labelsStep = UpdateSubscription(_labelsStep, "LabelsStep", ValueSpan / DefaultLabelsStepRatio);
            _ticksStep = UpdateSubscription(_ticksStep, "TicksStep", ValueSpan / DefaultTickStepRatio);

            OnPropertyChanged("Label");
            OnPropertyChanged("Value");
            OnPropertyChanged("Minimum");
            OnPropertyChanged("Maximum");
            OnPropertyChanged("LabelsStep");
            OnPropertyChanged("TicksStep");
        }

        private IEnumerable<IPropertyViewModel> GetConfigurationParameters()
        {
            var parameters = new IPropertyViewModel[]
            {
                new TextPropertyViewModel("Label", Configuration, _services),
                new TextPropertyViewModel("Value", Configuration, _services),
                new TextPropertyViewModel("Minimum", Configuration, _services),
                new TextPropertyViewModel("Maximum", Configuration, _services),
                new TextPropertyViewModel("Format", Configuration, _services),
                new DividerPropertyViewModel(),
                new ColourPropertyViewModel("LabelsStep", Configuration, _services),
                new ColourPropertyViewModel("TicksStep", Configuration, _services),
                new TextPropertyViewModel("Wedge", Configuration, _services),
                new TextPropertyViewModel("InnerRadius", Configuration, _services),
                new ColourPropertyViewModel("TicksColour", Configuration, _services),
                new DividerPropertyViewModel(),
                new TextPropertyViewModel("LowerSpan", Configuration, _services),
                new TextPropertyViewModel("UpperSpan", Configuration, _services),
                new ColourPropertyViewModel("LowerColour", Configuration, _services),
                new ColourPropertyViewModel("MidColour", Configuration, _services),
                new ColourPropertyViewModel("UpperColour", Configuration, _services),
            };
            return parameters;
        }

        public string Label => _label.String;

        public double Value => _value.Double;

        public double Maximum => _maximum.Double;

        public double Minimum => _minimum.Double;

        public string Format
        {
            get { return _format; }
            private set
            {
                if (_format != null && _format.Equals(value))
                {
                    return;
                }
                _format = value;
                OnPropertyChanged();
                OnPropertyChanged("Minimum");
                OnPropertyChanged("Maximum");
            }
        }

        public double LabelsStep => _labelsStep.Double;

        public double TicksStep => _ticksStep.Double;

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            Update();
        }

        private double ValueSpan => Maximum - Minimum;

        private void Update()
        {
            Format = NamedValues.GetString("Format", "{0}%");
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }

        public void InitialiseGuageView(AngularGauge view)
        {
            view.LabelFormatter = x => string.Format(Format,x);
            Update();

            var ticksColour = NamedValues.GetColour("TicksColour", "Gray");
            var wedge = NamedValues.GetDouble("Wedge", 300.0);
            view.Wedge = wedge;
            view.TicksForeground = new SolidColorBrush(ticksColour);

            InitialiseSpans(view);
        }

        private void InitialiseSpans(AngularGauge view)
        {
            var lowerSpan = NamedValues.GetDouble("LowerSpan", TicksStep);
            var upperSpan = NamedValues.GetDouble("UpperSpan", TicksStep);
            var midSpanStart = Minimum + lowerSpan;
            var midSpanEnd = Maximum - upperSpan;
            var lowerColour = NamedValues.GetColour("LowerColour", "LightGray");
            var midColour = NamedValues.GetColour("MidColour", "#F8A725");
            var upperColour = NamedValues.GetColour("UpperColour", "#FF3939");
            view.Sections.Add(new AngularSection() {FromValue = Minimum, ToValue = midSpanStart, Fill = new SolidColorBrush(lowerColour)});
            view.Sections.Add(new AngularSection() {FromValue = midSpanStart, ToValue = midSpanEnd, Fill = new SolidColorBrush(midColour)});
            view.Sections.Add(new AngularSection() {FromValue = midSpanEnd, ToValue = Maximum, Fill = new SolidColorBrush(upperColour)});
            view.SectionsInnerRadius = NamedValues.GetDouble("InnerRadius", 0.5);
        }
    }
}