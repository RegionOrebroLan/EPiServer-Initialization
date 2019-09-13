using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Linq;
using EPiServer.Data;
using EPiServer.Logging;
using RegionOrebroLan.Data;
using RegionOrebroLan.Data.Extensions;

namespace RegionOrebroLan.EPiServer.Initialization.Internal
{
	public class DatabaseInitializer : IDatabaseInitializer
	{
		#region Fields

		private static readonly IEnumerable<string> _validProviderNames = new[] {"System.Data.SqlClient"};

		#endregion

		#region Constructors

		public DatabaseInitializer(IApplicationDomain applicationDomain, IConnectionStringBuilderFactory connectionStringBuilderFactory, DataAccessOptions dataAccessOptions, IDatabaseManagerFactory databaseManagerFactory, IFileSystem fileSystem, ILoggerFactory loggerFactory)
		{
			this.ApplicationDomain = applicationDomain ?? throw new ArgumentNullException(nameof(applicationDomain));
			this.ConnectionStringBuilderFactory = connectionStringBuilderFactory ?? throw new ArgumentNullException(nameof(connectionStringBuilderFactory));
			this.DataAccessOptions = dataAccessOptions ?? throw new ArgumentNullException(nameof(dataAccessOptions));
			this.DatabaseManagerFactory = databaseManagerFactory ?? throw new ArgumentNullException(nameof(databaseManagerFactory));
			this.FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

			if(loggerFactory == null)
				throw new ArgumentNullException(nameof(loggerFactory));

			this.Logger = loggerFactory.Create(this.GetType().FullName);
		}

		#endregion

		#region Properties

		protected internal virtual IApplicationDomain ApplicationDomain { get; }
		protected internal virtual IConnectionStringBuilderFactory ConnectionStringBuilderFactory { get; }
		protected internal virtual DataAccessOptions DataAccessOptions { get; }
		protected internal virtual IDatabaseManagerFactory DatabaseManagerFactory { get; }
		protected internal virtual IFileSystem FileSystem { get; }
		protected internal virtual ILogger Logger { get; }
		protected internal virtual IEnumerable<string> ValidProviderNames => _validProviderNames;

		#endregion

		#region Methods

		protected internal virtual void CreateDatabaseIfNecessary(ConnectionStringOptions connectionSetting, IDatabaseManager databaseManager)
		{
			if(connectionSetting == null)
				throw new ArgumentNullException(nameof(connectionSetting));

			if(databaseManager == null)
				throw new ArgumentNullException(nameof(databaseManager));

			try
			{
				databaseManager.CreateDatabaseIfItDoesNotExistOrIfTheDatabaseFileDoesNotExist(connectionSetting.ConnectionString);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not ensure/create database for connection: name = \"{connectionSetting.Name}\", connection-string = \"{connectionSetting.ConnectionString}\" & provider-name = \"{connectionSetting.ProviderName}\".", exception);
			}
		}

		[SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
		public virtual void Initialize()
		{
			try
			{
				var databaseManagers = new Dictionary<string, IDatabaseManager>(StringComparer.OrdinalIgnoreCase);

				foreach(var connectionSetting in this.DataAccessOptions.ConnectionStrings.Where(item => !string.IsNullOrEmpty(item?.ProviderName) && this.ValidProviderNames.Contains(item.ProviderName, StringComparer.OrdinalIgnoreCase)))
				{
					if(!databaseManagers.TryGetValue(connectionSetting.ProviderName, out var databaseManager))
					{
						databaseManager = this.DatabaseManagerFactory.Create(connectionSetting.ProviderName);
						databaseManagers.Add(connectionSetting.ProviderName, databaseManager);
					}

					this.CreateDatabaseIfNecessary(connectionSetting, databaseManager);
				}
			}
			catch(Exception exception)
			{
				const string message = "Could not initialize databases.";

				if(this.Logger.IsErrorEnabled())
					this.Logger.Error(message, exception);

				throw new InvalidOperationException(message, exception);
			}
		}

		#endregion
	}
}