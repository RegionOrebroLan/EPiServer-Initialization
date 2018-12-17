using System;
using EPiServer.Data.SchemaUpdates;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using RegionOrebroLan.Data;
using RegionOrebroLan.Data.Common;
using RegionOrebroLan.EPiServer.Data.SchemaUpdates;
using RegionOrebroLan.EPiServer.Initialization.Internal;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[InitializableModule]
	public class DataInitialization : IConfigurableModule
	{
		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			this.ConfigureDataDirectory(context);
			this.ConfigureDatabase(context);
			this.ConfigureData(context);
		}

		protected internal virtual void ConfigureData(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.RemoveAll<ISchemaUpdater>();

			context.Services.AddSingleton<IProviderFactories, DbProviderFactoriesWrapper>();
			context.Services.AddSingleton<ISchemaUpdater, SchemaUpdater>();
		}

		protected internal virtual void ConfigureDatabase(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.AddSingleton<IConnectionStringBuilderFactory, ConnectionStringBuilderFactory>();
			context.Services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();
			context.Services.AddSingleton<IDatabaseManagerFactory, DatabaseManagerFactory>();
		}

		protected internal virtual void ConfigureDataDirectory(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.AddSingleton<IDataDirectoryInitializer, DataDirectoryInitializer>();
		}

		public virtual void Initialize(InitializationEngine context)
		{
			this.InitializeDataDirectory(context);
			this.InitializeDatabase(context);
			this.InitializeData(context);
		}

		protected internal virtual void InitializeData(InitializationEngine context) { }

		protected internal virtual void InitializeDatabase(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			var databaseInitializer = context.Locate.Advanced.GetInstance<IDatabaseInitializer>();

			databaseInitializer.Initialize();
		}

		protected internal virtual void InitializeDataDirectory(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			var dataDirectoryInitializer = context.Locate.Advanced.GetInstance<IDataDirectoryInitializer>();

			dataDirectoryInitializer.Initialize();
		}

		public virtual void Uninitialize(InitializationEngine context)
		{
			this.UninitializeData(context);
			this.UninitializeDatabase(context);
			this.UninitializeDataDirectory(context);
		}

		protected internal virtual void UninitializeData(InitializationEngine context) { }
		protected internal virtual void UninitializeDatabase(InitializationEngine context) { }
		protected internal virtual void UninitializeDataDirectory(InitializationEngine context) { }

		#endregion
	}
}