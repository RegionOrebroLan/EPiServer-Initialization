using System;
using EPiServer.Data;
using EPiServer.Logging;
using RegionOrebroLan.EPiServer.Data;

namespace RegionOrebroLan.EPiServer.Initialization.Internal
{
	public class DatabaseInitializer : IDatabaseInitializer
	{
		#region Constructors

		public DatabaseInitializer(DataAccessOptions dataAccessOptions, IDatabaseCreator databaseCreator, ILoggerFactory loggerFactory)
		{
			this.DataAccessOptions = dataAccessOptions ?? throw new ArgumentNullException(nameof(dataAccessOptions));
			this.DatabaseCreator = databaseCreator ?? throw new ArgumentNullException(nameof(databaseCreator));
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).Create(this.GetType().FullName);
		}

		#endregion

		#region Properties

		protected internal virtual DataAccessOptions DataAccessOptions { get; }
		protected internal virtual IDatabaseCreator DatabaseCreator { get; }
		protected internal virtual ILogger Logger { get; }

		#endregion

		#region Methods

		public virtual void Initialize()
		{
			foreach(var options in this.DataAccessOptions.ConnectionStrings)
			{
				this.DatabaseCreator.EnsureCreated(options);
			}
		}

		#endregion
	}
}