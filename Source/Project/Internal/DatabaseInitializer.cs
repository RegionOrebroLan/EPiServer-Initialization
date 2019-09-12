using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using EPiServer.Data;
using EPiServer.Logging.Compatibility;
using RegionOrebroLan.Data;
using RegionOrebroLan.Data.Extensions;

namespace RegionOrebroLan.EPiServer.Initialization.Internal
{
	public class DatabaseInitializer : IDatabaseInitializer
	{
		#region Fields

		private static readonly ILog _logger = LogManager.GetLogger(typeof(DatabaseInitializer));
		private static readonly IEnumerable<string> _validProviderNames = new[] {"System.Data.SqlClient"};

		#endregion

		#region Constructors

		public DatabaseInitializer(IApplicationDomain applicationDomain, IConnectionStringBuilderFactory connectionStringBuilderFactory, DataAccessOptions dataAccessOptions, IDatabaseManagerFactory databaseManagerFactory, IFileSystem fileSystem) : this(applicationDomain, connectionStringBuilderFactory, dataAccessOptions, databaseManagerFactory, fileSystem, _logger) { }

		protected internal DatabaseInitializer(IApplicationDomain applicationDomain, IConnectionStringBuilderFactory connectionStringBuilderFactory, DataAccessOptions dataAccessOptions, IDatabaseManagerFactory databaseManagerFactory, IFileSystem fileSystem, ILog logger)
		{
			this.ApplicationDomain = applicationDomain ?? throw new ArgumentNullException(nameof(applicationDomain));
			this.ConnectionStringBuilderFactory = connectionStringBuilderFactory ?? throw new ArgumentNullException(nameof(connectionStringBuilderFactory));
			this.DataAccessOptions = dataAccessOptions ?? throw new ArgumentNullException(nameof(dataAccessOptions));
			this.DatabaseManagerFactory = databaseManagerFactory ?? throw new ArgumentNullException(nameof(databaseManagerFactory));
			this.FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
			this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		#endregion

		#region Properties

		protected internal virtual IApplicationDomain ApplicationDomain { get; }
		protected internal virtual IConnectionStringBuilderFactory ConnectionStringBuilderFactory { get; }
		protected internal virtual DataAccessOptions DataAccessOptions { get; }
		protected internal virtual IDatabaseManagerFactory DatabaseManagerFactory { get; }
		protected internal virtual IFileSystem FileSystem { get; }
		protected internal virtual ILog Logger { get; }
		protected internal virtual IEnumerable<string> ValidProviderNames => _validProviderNames;

		#endregion

		#region Methods

		public virtual void Initialize()
		{
			var databaseManagers = new Dictionary<string, IDatabaseManager>(StringComparer.OrdinalIgnoreCase);

			foreach(var connectionSetting in this.DataAccessOptions.ConnectionStrings.Where(item => !string.IsNullOrEmpty(item?.ProviderName) && this.ValidProviderNames.Contains(item.ProviderName, StringComparer.OrdinalIgnoreCase)))
			{
				if(!databaseManagers.TryGetValue(connectionSetting.ProviderName, out var databaseManager))
				{
					databaseManager = this.DatabaseManagerFactory.Create(connectionSetting.ProviderName);
					databaseManagers.Add(connectionSetting.ProviderName, databaseManager);
				}

				databaseManager.CreateDatabaseIfItDoesNotExistOrIfTheDatabaseFileDoesNotExist(connectionSetting.ConnectionString);
			}
		}

		#endregion
	}
}