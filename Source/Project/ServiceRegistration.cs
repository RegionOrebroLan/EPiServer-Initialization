using System;
using System.IO.Abstractions;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;

namespace RegionOrebroLan.EPiServer.Initialization
{
	/// <summary>
	/// Service-registration of some general types used in our EPiServer add-ons.
	/// </summary>
	[InitializableModule]
	public class ServiceRegistration : IConfigurableModule
	{
		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.AddSingleton(AppDomain.CurrentDomain);
			context.Services.AddSingleton<IFileSystem, FileSystem>();
			context.Services.AddSingleton(LogManager.LoggerFactory());
		}

		public virtual void Initialize(InitializationEngine context) { }
		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}