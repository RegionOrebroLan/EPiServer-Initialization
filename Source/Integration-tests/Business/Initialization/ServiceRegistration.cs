using System;
using System.IO.Abstractions;
using EPiServer.Approvals;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Hosting;
using RegionOrebroLan.EPiServer.Initialization.IntegrationTests.Business.Approvals;
using RegionOrebroLan.EPiServer.Initialization.IntegrationTests.Business.Web.Hosting;

namespace RegionOrebroLan.EPiServer.Initialization.IntegrationTests.Business.Initialization
{
	[InitializableModule]
	public class ServiceRegistration : IConfigurableModule
	{
		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.AddSingleton(AppDomain.CurrentDomain);
			context.Services.AddSingleton<IApprovalEngine, FakedApprovalEngine>();
			context.Services.AddSingleton<IApprovalRepository, FakedApprovalRepository>();
			context.Services.AddSingleton<IFileSystem, FileSystem>();

			context.ConfigurationComplete += (sender, e) => { e.Services.AddSingleton<IHostingEnvironment, FakedHostingEnvironment>(); };
		}

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			GenericHostingEnvironment.Instance = context.Locate.Advanced.GetInstance<IHostingEnvironment>();
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}