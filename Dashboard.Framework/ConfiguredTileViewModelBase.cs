using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework
{
    public abstract class ConfiguredTileViewModelBase : NotifyingViewModelBase
    {
        protected INamedValueRepository Configuration;
        protected INamedValueRepository NamedValues;

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
    }
}