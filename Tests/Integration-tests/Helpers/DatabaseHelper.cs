using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using EPiServer.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using RegionOrebroLan.EPiServer.Data;
using RegionOrebroLan.EPiServer.Data.Hosting;
using RegionOrebroLan.EPiServer.Data.SqlClient.Extensions;

namespace IntegrationTests.Helpers
{
	public static class DatabaseHelper
	{
		#region Fields

		private static readonly ConnectionStringSettings _epiServerConnectionSetting = ConfigurationManager.ConnectionStrings[EPiServerDataStoreSection.DefaultConnectionStringName];

		private static readonly IHostEnvironment _hostEnvironment = new HostEnvironment
		{
			ContentRootPath = Global.ProjectDirectoryPath
		};

		#endregion

		#region Methods

		private static async Task<DbContext> CreateContextAsync(string connectionString)
		{
			connectionString = SqlConnectionStringBuilderExtension.ResolveConnectionString(connectionString, _hostEnvironment);

			var contextOptionsBuilder = new DbContextOptionsBuilder<DbContext>();

			contextOptionsBuilder.UseSqlServer(connectionString);

			return await Task.FromResult(new DbContext(contextOptionsBuilder.Options));
		}

		public static async Task CreateDatabaseAsync(string connectionString)
		{
			// ReSharper disable All
			using(var context = await CreateContextAsync(connectionString))
			{
				await context.Database.EnsureCreatedAsync();
			}
			// ReSharper restore All
		}

		public static async Task DeleteDatabaseAsync(string connectionString)
		{
			// ReSharper disable All
			using(var context = await CreateContextAsync(connectionString))
			{
				await context.Database.EnsureDeletedAsync();
			}
			// ReSharper restore All
		}

		public static async Task DropLocalDatabasesAsync()
		{
			var dataDirectory = AppDomainHelper.GetDataDirectory();
			AppDomainHelper.SetDefaultDataDirectory();

			foreach(var connectionStringSettings in ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>())
			{
				if(!string.Equals(connectionStringSettings.ProviderName, ProviderNames.SqlServer, StringComparison.OrdinalIgnoreCase))
					continue;

				await DeleteDatabaseAsync(connectionStringSettings.ConnectionString);
			}

			AppDomainHelper.SetDataDirectory(dataDirectory);
		}

		public static async Task<bool> EPiServerDatabaseExistsAsync()
		{
			bool exists;

			var dataDirectory = AppDomainHelper.GetDataDirectory();
			AppDomainHelper.SetDefaultDataDirectory();

			// ReSharper disable All
			using(var context = await CreateContextAsync(_epiServerConnectionSetting.ConnectionString))
			{
				exists = await context.Database.CanConnectAsync();
			}
			// ReSharper restore All

			AppDomainHelper.SetDataDirectory(dataDirectory);

			return exists;
		}

		#endregion
	}
}