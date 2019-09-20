using System;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Initialization.Internal;
using EPiServer.ServiceLocation;
using RegionOrebroLan.EPiServer.Initialization.Configuration;
using RegionOrebroLan.EPiServer.Initialization.Internal;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[InitializableModule]
	[ModuleDependency(typeof(ModelSyncInitialization))]
	public class DefaultSiteContentInitialization : DisableableInitialization, IConfigurableModule
	{
		#region Constructors

		public DefaultSiteContentInitialization() : this(new DisableableInitializationConfiguration()) { }
		public DefaultSiteContentInitialization(IDisableableInitializationConfiguration configuration) : base(configuration) { }

		#endregion

		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			if(this.Disabled)
				return;

			context.Services.AddSingleton<IDefaultSiteContentInitializer, DefaultSiteContentInitializer>();
		}

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			if(this.Disabled)
				return;

			var defaultSiteContentInitializer = context.Locate.Advanced.GetInstance<IDefaultSiteContentInitializer>();

			defaultSiteContentInitializer.Initialize();
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}