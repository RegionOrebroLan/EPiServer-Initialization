using System;
using System.IO.Abstractions;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using RegionOrebroLan.EPiServer.Initialization.Configuration;

namespace RegionOrebroLan.EPiServer.Initialization
{
	/// <summary>
	/// Service-registration of some general types used in our EPiServer add-ons.
	/// </summary>
	[InitializableModule]
	[ModuleDependency(typeof(FrameworkInitialization))]
	public class ServiceRegistration : DisableableInitialization, IConfigurableModule
	{
		#region Constructors

		public ServiceRegistration() : this(new DisableableInitializationConfiguration()) { }
		public ServiceRegistration(IDisableableInitializationConfiguration configuration) : base(configuration) { }

		#endregion

		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			if(this.Disabled)
				return;

			context.Services.AddSingleton(AppDomain.CurrentDomain);
			context.Services.AddSingleton<IFileSystem, FileSystem>();
			context.Services.AddSingleton(serviceLocator =>
			{
				// LogManager.LoggerFactory() is initialized in EPiServer.Framework.Initialization.Internal.LoggerInitialization in assembly EPiServer.Framework.AspNet.
				// If it has not been initialized it will return null, so we fallback to TraceLoggerFactory.
				// ReSharper disable ConvertToLambdaExpression
				return LogManager.LoggerFactory() ?? new TraceLoggerFactory();
				// ReSharper restore ConvertToLambdaExpression
			});
		}

		public virtual void Initialize(InitializationEngine context) { }
		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}