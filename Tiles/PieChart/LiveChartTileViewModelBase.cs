using System.Collections.Generic;
using LiveCharts.Defaults;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Tiles.PieChart
{
    internal class LiveChartTileViewModelBase : ConfiguredTileViewModelBase
    {
        protected LiveChartTileViewModelBase(ITileProperties properties) : base(properties)
        {
        }

        protected IEnumerable<IDataValue> UpdateSubscription(IEnumerable<IDataValue> existingValues, string name, ICollection<ObservableValue> observableCollection)
        {
            observableCollection.Clear();
            foreach (var dataValue in existingValues)
            {
                dataValue.Broadcaster.RemoveListener(this);
            }

            var newValues = NamedValues.GetDatums(name);
            foreach (var dataValue in newValues)
            {
                var chartValue = new ObservableValue(dataValue.Double);
                new LiveChartsObervableValueAdapter(dataValue, chartValue);
                observableCollection.Add(chartValue);
            }

            return newValues;
        }
    }
}