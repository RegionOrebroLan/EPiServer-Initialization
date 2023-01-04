using System;
using System.Data.SqlClient;
using EPiServer.Data;
using Microsoft.EntityFrameworkCore;
using RegionOrebroLan.EPiServer.Data;
using RegionOrebroLan.EPiServer.Data.SqlClient.Extensions;

namespace IntegrationTests.Helpers
{
	public class SqlServerLocalDatabaseCreator : IDatabaseCreator
	{
		#region Constructors

		public SqlServerLocalDatabaseCreator(IConnectionStringResolver connectionStringResolver)
		{
			this.ConnectionStringResolver = connectionStringResolver ?? throw new ArgumentNullException(nameof(connectionStringResolver));
		}

		#endregion

		#region Properties

		protected internal virtual IConnectionStringResolver ConnectionStringResolver { get; }

		#endregion

		#region Methods

		protected internal virtual DbContext CreateContext(string connectionString)
		{
			var contextOptionsBuilder = new DbContextOptionsBuilder<DbContext>();

			contextOptionsBuilder.UseSqlServer(connectionString);

			return new DbContext(contextOptionsBuilder.Options);
		}

		public virtual bool EnsureCreated(ConnectionStringOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(!string.Equals(options.ProviderName, ProviderNames.SqlServer, StringComparison.OrdinalIgnoreCase))
				return false;

			var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(options.ConnectionString);

			if(!sqlConnectionStringBuilder.IsLocalDatabaseConnectionString())
				return false;

			// We make a copy. If the connection-string is resolved we only change the copy.
			options = new ConnectionStringOptions
			{
				ConnectionString = string.Copy(options.ConnectionString),
				Name = string.Copy(options.Name),
				ProviderName = string.Copy(options.ProviderName)
			};

			this.ConnectionStringResolver.Resolve(options);

			// ReSharper disable ConvertToUsingDeclaration
			using(var context = this.CreateContext(options.ConnectionString))
			{
				return context.Database.EnsureCreated();
			}
			// ReSharper restore ConvertToUsingDeclaration
		}

		#endregion
	}
}