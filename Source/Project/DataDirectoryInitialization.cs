using System;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[InitializableModule]
	public class DataDirectoryInitialization : IConfigurableModule
	{
		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.AddSingleton<IDataDirectoryInitializer, DataDirectoryInitializer>();
		}

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			var dataDirectoryInitializer = context.Locate.Advanced.GetInstance<IDataDirectoryInitializer>();

			dataDirectoryInitializer.Initialize();
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}