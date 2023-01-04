using System;
using EPiServer.Data.SchemaUpdates;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using RegionOrebroLan.EPiServer.Data;
using RegionOrebroLan.EPiServer.Data.Common;
using RegionOrebroLan.EPiServer.Data.Hosting;
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
				this._initializeDatabaseDisabled ??= this.Configuration.IsDisabled(this.CreateInitializationKey(nameof(this.InitializeDatabase))) ?? this.Disabled;

				// ReSharper disable PossibleInvalidOperationException
				return this._initializeDatabaseDisabled.Value;
				// ReSharper restore PossibleInvalidOperationException
			}
		}

		protected internal virtual bool InitializeDataDirectoryDisabled
		{
			get
			{
				this._initializeDataDirectoryDisabled ??= this.Configuration.IsDisabled(this.CreateInitializationKey(nameof(this.InitializeDataDirectory))) ?? this.Disabled;

				// ReSharper disable PossibleInvalidOperationException
				return this._initializeDataDirectoryDisabled.Value;
				// ReSharper restore PossibleInvalidOperationException
			}
		}

		protected internal virtual bool InitializeDataDisabled
		{
			get
			{
				this._initializeDataDisabled ??= this.Configuration.IsDisabled(this.CreateInitializationKey(nameof(this.InitializeData))) ?? this.Disabled;

				// ReSharper disable PossibleInvalidOperationException
				return this._initializeDataDisabled.Value;
				// ReSharper restore PossibleInvalidOperationException
			}
		}

		#endregion

		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			if(this.InitializeDataDirectoryDisabled && this.InitializeDataDisabled && this.InitializeDatabaseDisabled)
				return;

			context.Services.TryAdd<IHostEnvironment, HostEnvironment>(ServiceInstanceScope.Singleton);
			context.Services.TryAdd(_ => LogManager.LoggerFactory() ?? new TraceLoggerFactory(), ServiceInstanceScope.Singleton);

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

			context.Services.TryAdd<IDbProviderFactories, DbProviderFactoriesWrapper>(ServiceInstanceScope.Singleton);

			context.Services.RemoveAll<ISchemaUpdater>();
			context.Services.AddTransient<ISchemaUpdater, SchemaUpdater>();
		}

		protected internal virtual void ConfigureDatabase(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.TryAdd<IConnectionStringResolver, ConnectionStringResolver>(ServiceInstanceScope.Singleton);
			context.Services.TryAdd<IDatabaseInitializer, DatabaseInitializer>(ServiceInstanceScope.Singleton);
		}

		protected internal virtual void ConfigureDataDirectory(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.TryAdd<IDataDirectoryInitializer, DataDirectoryInitializer>(ServiceInstanceScope.Singleton);
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