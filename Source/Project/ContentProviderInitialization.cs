using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[InitializableModule]
	[ModuleDependency(typeof(DefaultSiteContentInitialization))]
	public class ContentProviderInitialization : IInitializableModule
	{
		#region Methods

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

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