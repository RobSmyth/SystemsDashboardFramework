using System.Collections.Generic;
using System.Linq;
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

        protected IEnumerable<IDataValue> UpdateSubscriptions(IEnumerable<IDataValue> existingValues, string name, ICollection<ObservableValue> observableCollection)
        {
            Flush(existingValues, observableCollection);

            var newValues = NamedValues.GetDatums(name).ToArray();
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