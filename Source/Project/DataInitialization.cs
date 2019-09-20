using System;
using EPiServer.Data.SchemaUpdates;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using RegionOrebroLan.Data;
using RegionOrebroLan.Data.Common;
using RegionOrebroLan.EPiServer.Data.SchemaUpdates;
using RegionOrebroLan.EPiServer.Initialization.Configuration;
using RegionOrebroLan.EPiServer.Initialization.Internal;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[InitializableModule]
	public class DataInitialization : DisableableInitialization, IConfigurableModule
	{
		#region Fields

		private bool? _initializeDatabaseDisabled;
		private bool? _initializeDataDirectoryDisabled;
		private bool? _initializeDataDisabled;

		#endregion

		#region Constructors

		public DataInitialization() : this(new DisableableInitializationConfiguration()) { }
		public DataInitialization(IDisableableInitializationConfiguration configuration) : base(configuration) { }

		#endregion

		#region Properties

		protected internal virtual bool InitializeDatabaseDisabled
		{
			get
			{
				if(this._initializeDatabaseDisabled == null)
					this._initializeDatabaseDisabled = this.Configuration.IsDisabled(this.CreateInitializationKey(nameof(this.InitializeDatabase))) ?? this.Disabled;

				return this._initializeDatabaseDisabled.Value;
			}
		}

		protected internal virtual bool InitializeDataDirectoryDisabled
		{
			get
			{
				if(this._initializeDataDirectoryDisabled == null)
					this._initializeDataDirectoryDisabled = this.Configuration.IsDisabled(this.CreateInitializationKey(nameof(this.InitializeDataDirectory))) ?? this.Disabled;

				return this._initializeDataDirectoryDisabled.Value;
			}
		}

		protected internal virtual bool InitializeDataDisabled
		{
			get
			{
				if(this._initializeDataDisabled == null)
					this._initializeDataDisabled = this.Configuration.IsDisabled(this.CreateInitializationKey(nameof(this.InitializeData))) ?? this.Disabled;

				return this._initializeDataDisabled.Value;
			}
		}

		#endregion

		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(!this.InitializeDataDirectoryDisabled)
				this.ConfigureDataDirectory(context);

			if(!this.InitializeDatabaseDisabled)
				this.ConfigureDatabase(context);

			if(!this.InitializeDataDisabled)
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
			if(!this.InitializeDataDirectoryDisabled)
				this.InitializeDataDirectory(context);

			if(!this.InitializeDatabaseDisabled)
				this.InitializeDatabase(context);

			if(!this.InitializeDataDisabled)
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
			if(!this.InitializeDataDisabled)
				this.UninitializeData(context);

			if(!this.InitializeDatabaseDisabled)
				this.UninitializeDatabase(context);

			if(!this.InitializeDataDirectoryDisabled)
				this.UninitializeDataDirectory(context);
		}

		protected internal virtual void UninitializeData(InitializationEngine context) { }
		protected internal virtual void UninitializeDatabase(InitializationEngine context) { }
		protected internal virtual void UninitializeDataDirectory(InitializationEngine context) { }

		#endregion
	}
}