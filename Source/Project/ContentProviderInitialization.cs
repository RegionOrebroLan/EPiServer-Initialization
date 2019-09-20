using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using RegionOrebroLan.EPiServer.Initialization.Configuration;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[InitializableModule]
	[ModuleDependency(typeof(DefaultSiteContentInitialization))]
	public class ContentProviderInitialization : DisableableInitialization, IInitializableModule
	{
		#region Constructors

		public ContentProviderInitialization() : this(new DisableableInitializationConfiguration()) { }
		public ContentProviderInitialization(IDisableableInitializationConfiguration configuration) : base(configuration) { }

		#endregion

		#region Methods

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			if(this.Disabled)
				return;

			var customContentProviders = context.Locate.Advanced.GetInstance<IDictionary<string, ContentProviderBuilder>>();
			var contentProviderManager = context.Locate.Advanced.GetInstance<IContentProviderManager>();

			foreach(var item in customContentProviders)
			{
				contentProviderManager.ProviderMap.AddProvider(item.Value.Build(context.Locate.Advanced));
			}
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}