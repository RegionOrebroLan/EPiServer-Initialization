using System;
using EPiServer.Data.SchemaUpdates;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using RegionOrebroLan.Data.Common;
using RegionOrebroLan.EPiServer.Data.SchemaUpdates;
using RegionOrebroLan.EPiServer.Framework;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[BeforeModuleDependency(typeof(global::EPiServer.Data.DataInitialization), typeof(global::EPiServer.Validation.Internal.ValidationService))]
	[CLSCompliant(false)]
	[InitializableModule]
	[ModuleDependency(typeof(DatabaseInitialization), typeof(ServiceContainerInitialization))]
	public class DataInitialization : IConfigurableModule
	{
		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.RemoveAll<ISchemaUpdater>();

			context.Services.AddSingleton<IProviderFactories, DbProviderFactoriesWrapper>();
			context.Services.AddSingleton<ISchemaUpdater, SchemaUpdater>();
		}

		public virtual void Initialize(InitializationEngine context) { }
		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}