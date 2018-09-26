using System;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Initialization.Internal;
using EPiServer.ServiceLocation;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[InitializableModule]
	[ModuleDependency(typeof(ModelSyncInitialization))]
	public class DefaultSiteContentInitialization : IConfigurableModule
	{
		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.AddSingleton<IDefaultSiteContentInitializer, DefaultSiteContentInitializer>();
		}

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			var defaultSiteContentInitializer = context.Locate.Advanced.GetInstance<IDefaultSiteContentInitializer>();

			defaultSiteContentInitializer.Initialize();
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}