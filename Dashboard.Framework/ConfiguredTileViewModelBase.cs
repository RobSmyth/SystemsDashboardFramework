using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework
{
    public abstract class ConfiguredTileViewModelBase : NotifyingViewModelBase
    {
        protected readonly INamedValueRepository Configuration;
        protected readonly INamedValueRepository NamedValues;

        protected ConfiguredTileViewModelBase(ITileProperties properties)
        {
            Configuration = properties.Properties;
            NamedValues = properties.NamedValueRepository;
        }

        protected IDataValue UpdateSubscription(IDataValue existing, string propertyName, object defaultValue)
        {
            var datum = string.IsNullOrWhiteSpace(propertyName) ? (IDataValue)new NullDataValue() : NamedValues.GetDatum(propertyName, defaultValue);
            if (ReferenceEquals(existing, datum))
            {
                return existing;
            }

            if (datum.NotSet)
            {
                datum.Instance = defaultValue;
            }

            existing.Broadcaster.RemoveListener(this);
            datum.Broadcaster.AddListener(this, () => OnPropertyChanged(propertyName));

            return datum;
        }

        protected void Flush<T>(IEnumerable<IDataValue> existingValues, ICollection<T> observableCollection)
        {
            observableCollection.Clear();
            foreach (var dataValue in existingValues)
            {
                dataValue.Broadcaster.RemoveListener(this);
            }
        }

        protected IEnumerable<IDataValue> UpdateSubscription<T>(IEnumerable<IDataValue> existingValues, string name, ICollection<T> observableCollection, Func<IDataValue, T> converterFunc)
        {
            Flush(existingValues, observableCollection);

            var newValues = NamedValues.GetDatums(name).ToArray();
            foreach (var dataValue in newValues)
            {
                observableCollection.Add(converterFunc(dataValue));
            }

            return newValues;
        }
    }
}