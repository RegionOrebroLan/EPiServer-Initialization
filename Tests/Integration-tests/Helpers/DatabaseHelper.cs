using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Abstractions;
using EPiServer.Data.Configuration;
using RegionOrebroLan;
using RegionOrebroLan.Data;
using RegionOrebroLan.Data.Common;
using RegionOrebroLan.Data.Extensions;
using RegionOrebroLan.Data.SqlClient;

namespace IntegrationTests.Helpers
{
	public static class DatabaseHelper
	{
		#region Fields

		private static readonly IProviderFactories _providerFactories = new DbProviderFactoriesWrapper();
		private static readonly IDatabaseManagerFactory _databaseManagerFactory = new DatabaseManagerFactory(new AppDomainWrapper(AppDomain.CurrentDomain), new ConnectionStringBuilderFactory(_providerFactories), new FileSystem(), _providerFactories);
		private static readonly ConnectionStringSettings _epiServerConnectionSetting = ConfigurationManager.ConnectionStrings[EPiServerDataStoreSection.DefaultConnectionStringName];
		private static readonly string _epiServerProviderName = _epiServerConnectionSetting.ProviderName;
		private static readonly string _epiServerConnectionString = ResolveConnectionString(_epiServerConnectionSetting.ConnectionString);
		private static readonly ConnectionStringBuilder _epiServerConnectionStringBuilder = new ConnectionStringBuilder(_epiServerConnectionString);
		private static readonly string _epiServerDatabaseFilePath = _epiServerConnectionStringBuilder.DatabaseFilePath;
		private static readonly string _epiServerDatabaseLogFilePath = _epiServerDatabaseFilePath.Substring(0, _epiServerDatabaseFilePath.Length - 4) + "_log.ldf";
		private static readonly IDatabaseManager _epiServerDatabaseManager = _databaseManagerFactory.Create(_epiServerProviderName);

		#endregion

		#region Methods

		public static void CreateEPiServerDatabase()
		{
			_epiServerDatabaseManager.CreateDatabase(_epiServerConnectionString);
		}

		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		public static void DropDatabasesIfTheyExist()
		{
			foreach(ConnectionStringSettings connectionSetting in ConfigurationManager.ConnectionStrings)
			{
				IDatabaseManager databaseManager;

				try
				{
					databaseManager = _databaseManagerFactory.Create(connectionSetting.ProviderName);
				}
				catch(InvalidOperationException)
				{
					continue;
				}

				var connectionString = ResolveConnectionString(connectionSetting.ConnectionString);
				databaseManager.DropDatabaseIfItExists(connectionString);
			}
		}

		public static void DropEPiServerDatabaseFile()
		{
			File.Delete(_epiServerDatabaseFilePath);
		}

		public static void DropEPiServerDatabaseFiles()
		{
			DropEPiServerDatabaseFile();
			DropEPiServerDatabaseLogFile();
		}

		public static void DropEPiServerDatabaseIfItExists()
		{
			_epiServerDatabaseManager.DropDatabaseIfItExists(_epiServerConnectionString);
		}

		public static void DropEPiServerDatabaseLogFile()
		{
			File.Delete(_epiServerDatabaseLogFilePath);
		}

		public static bool EPiServerDatabaseExists()
		{
			return _epiServerDatabaseManager.DatabaseExists(_epiServerConnectionString);
		}

		public static bool EPiServerDatabaseFileExists()
		{
			return File.Exists(_epiServerDatabaseFilePath);
		}

		public static bool EPiServerDatabaseLogFileExists()
		{
			return File.Exists(_epiServerDatabaseLogFilePath);
		}

		private static string ResolveConnectionString(string connectionString)
		{
			return connectionString?.Replace("|DataDirectory|", Path.Combine(Global.ProjectDirectoryPath, "App_Data\\"));
		}

		#endregion
	}
}