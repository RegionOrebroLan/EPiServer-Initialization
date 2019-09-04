using System;
using System.Configuration;
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
		#region Properties

		protected internal virtual bool DatabaseInitializationEnabled { get; } = IsFeatureEnabled("InitializeDatabase");
		protected internal virtual bool DataDirectoryInitializationEnabled { get; } = IsFeatureEnabled("InitializeDataDirectory");
		protected internal virtual bool DataInitializationEnabled { get; } = IsFeatureEnabled("InitializeData");

		#endregion

		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(this.DataDirectoryInitializationEnabled)
				this.ConfigureDataDirectory(context);

			if(this.DatabaseInitializationEnabled)
				this.ConfigureDatabase(context);

			if(this.DataInitializationEnabled)
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
			if(this.DataDirectoryInitializationEnabled)
				this.InitializeDataDirectory(context);

			if(this.DatabaseInitializationEnabled)
				this.InitializeDatabase(context);

			if(this.DataInitializationEnabled)
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

		private static bool IsFeatureEnabled(string feature)
		{
			if(!bool.TryParse(ConfigurationManager.AppSettings["RegionOrebroLan.EPiServer.Initialization:" + feature], out var enabled))
				enabled = true;

			return enabled;
		}

		public virtual void Uninitialize(InitializationEngine context)
		{
			if(this.DataInitializationEnabled)
				this.UninitializeData(context);

			if(this.DatabaseInitializationEnabled)
				this.UninitializeDatabase(context);

			if(this.DataDirectoryInitializationEnabled)
				this.UninitializeDataDirectory(context);
		}

		protected internal virtual void UninitializeData(InitializationEngine context) { }
		protected internal virtual void UninitializeDatabase(InitializationEngine context) { }
		protected internal virtual void UninitializeDataDirectory(InitializationEngine context) { }

		#endregion
	}
}