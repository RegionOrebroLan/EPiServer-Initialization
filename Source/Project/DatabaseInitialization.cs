using System;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using RegionOrebroLan.Data;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[InitializableModule]
	[ModuleDependency(typeof(DataDirectoryInitialization))]
	public class DatabaseInitialization : IConfigurableModule
	{
		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.AddSingleton<IConnectionStringBuilderFactory, ConnectionStringBuilderFactory>();
			context.Services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();
			context.Services.AddSingleton<IDatabaseManagerFactory, DatabaseManagerFactory>();
		}

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			var databaseInitializer = context.Locate.Advanced.GetInstance<IDatabaseInitializer>();

			databaseInitializer.Initialize();
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}