using LiveCharts.Defaults;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Tiles.PieChart
{
    public sealed class LiveChartsObervableValueAdapter
    {
        private readonly ObservableValue _observableChartPoint;
        private readonly IDataValue _dataValue;

        public LiveChartsObervableValueAdapter(IDataValue dataValue, ObservableValue observableChartPoint)
        {
            _dataValue = dataValue;
            _observableChartPoint = observableChartPoint;
            _observableChartPoint.PointChanged += ObservableChartPointChanged;
            _dataValue.Broadcaster.AddListener(this, OnDataValueChanged);
            OnDataValueChanged();
        }

        private void ObservableChartPointChanged()
        {
            _dataValue.Double = _observableChartPoint.Value;
        }

        private void OnDataValueChanged()
        {
            _observableChartPoint.Value = _dataValue.Double;
        }
    }
}